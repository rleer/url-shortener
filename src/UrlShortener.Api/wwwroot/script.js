async function shortenUrl() {
    const urlInput = document.getElementById("urlInput").value;
    if (!urlInput) return;

    const response = await fetch("/shorten", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ url: urlInput })
    });

    if (response.ok) {
        const data = await response.json();
        document.getElementById("result").innerHTML = `
            Shortened URL: <a href="${data.shortenedUrl}" target="_blank">${data.shortenedUrl}</a>
            <button onclick="copyToClipboard('${data.shortenedUrl}')">Copy</button>
        `;
    } else {
        document.getElementById("result").innerHTML = "Error: Invalid URL";
    }
}

function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        const copyMessage = document.getElementById("copy-message");
        copyMessage.hidden = false;
        setTimeout(() => copyMessage.hidden = true, 3000);
    }).catch(err => {
        console.error("Failed to copy: ", err);
    });
}
