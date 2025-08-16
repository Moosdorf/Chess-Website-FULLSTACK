using CsvHelper;
using DataLayer.Entities.Chess;
using DataLayer.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.csv_scripts;

public static class CsvImportScript
{
    public static void Run(ChessContext context)
    {
        using var reader = new StreamReader("lichess_puzzles.txt");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        const int batchSize = 10000;
        var batch = new List<Puzzle>();

        // Step 1: Load all existing tags into a dictionary
        var existingTags = context.Tags
            .AsNoTracking()
            .ToDictionary(t => t.Name, t => t);

        foreach (var record in csv.GetRecords<PuzzleScript>())
        {
            var puzzle = new Puzzle
            {
                PuzzleId = record.PuzzleId,
                FEN = record.FEN,
                Moves = record.Moves,
                Rating = record.Rating,
                RatingDeviation = record.RatingDeviation,
                Popularity = record.Popularity,
                NbPlays = record.NbPlays,
                Themes = record.Themes,
                GameUrl = record.GameUrl,
                OpeningTags = record.OpeningTags
            };

            var splitThemes = record.Themes.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            foreach (var tagName in splitThemes)
            {
                if (!existingTags.TryGetValue(tagName, out var tag))
                {
                    tag = new Tag { Name = tagName };
                    existingTags[tagName] = tag;
                    context.Tags.Add(tag); // queue for insert
                }
                puzzle.PuzzleTags.Add(new PuzzleTag { Puzzle = puzzle, Tag = tag });
            }

            batch.Add(puzzle);

            if (batch.Count >= batchSize)
            {
                context.Puzzles.AddRange(batch);
                context.SaveChanges();
                context.ChangeTracker.Clear();
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            context.Puzzles.AddRange(batch);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }

}
