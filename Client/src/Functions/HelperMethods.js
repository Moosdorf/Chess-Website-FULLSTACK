export function GetCookies() {
    var cookie = document.cookie;
    var keyvalues = cookie.split(";");
    var cookieData = {};
    keyvalues.forEach(kv => {
        kv = kv.split("=");
        if (kv.length === 2) {
            cookieData[kv[0]] = kv[1];
        }
    })

    return cookieData;
} 


export function ClearCookies() {
  const cookies = document.cookie.split(";");

  for (const cookie of cookies) {
    const eqPos = cookie.indexOf("=");
    const name = eqPos > -1 ? cookie.substring(0, eqPos).trim() : cookie.trim();

    // Expire the cookie at all possible paths
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/`;
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; domain=${window.location.hostname}`;
  }
}