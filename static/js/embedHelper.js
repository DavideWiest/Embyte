
function getiFrameWidth(id) {
    var iframe = document.querySelector("iframe#" + id)

    var win = iframe.contentWindow
    var doc = win.document
    var html = doc.documentElement
    var body = doc.body

    if (body) {
        //body.style.overflowX = "scroll" // scrollbar-jitter fix
        //body.style.overflowY = "hidden"
    }
    if (html) {
        //html.style.overflowX = "scroll" // scrollbar-jitter fix
        //html.style.overflowY = "hidden"
        var style = win.getComputedStyle(html)
        width = parseInt(style.getPropertyValue("width")) // round value
        height = parseInt(style.getPropertyValue("height"))
    }

    return { Width: width, Height: height }
}
