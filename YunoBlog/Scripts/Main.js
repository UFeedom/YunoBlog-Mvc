var autoplay = false;

var blog = window.blog = {
    autoHighlight: function () {
        if (window.enable_autoHighlight == undefined) {
            window.enable_autoHighlight = false
        }
        if (!window.enable_autoHighlight) {
            return
        }
        var a = null;
        $(window).scroll(function () {
            var c = $(window).scrollTop();
            var b = $(window).height();
            $(window.enable_autoHighlight).each(function () {
                var d = c + (b * 0.35);
                if (d > $(this).offset().top && d < $(this).offset().top + $(this).height()) {
                    if (a !== null) {
                        a.removeClass("active")
                    }
                    a = $(this).addClass("active");
                    return false
                }
            })
        }).scroll()
    },
    init: function () {
        var a = blog;
        a.autoHighlight();
        MusicPlayerEnabled();
    }
};
var lock = false;

function LoadArticles() {
    if (lock) return;
    lock = true;
    $.getJSON("/Home/GetArticles", {
        page: page,
        year: year,
        month: month,
        category: category,
        title: title
        }, function (data) {
            for (var i in data)
            {
                var article = "<div class='article'><div class='header'><div class='title'><a href='/Page/" + data[i].ID + "'>{Article_Title}</a></div><div class='info info1'><span class='time'>{Article_CreationTime} 发表在 <a href='/Category/{Article_CategoryID}'>{Article_Category}</a></span></div></div><div class='section entry'>{Article_Html}{Article_More}</div></div>"
                .replace("{Article_More}", data[i].Line < 20 ? "" : "<p><a href='/Page/" + data[i].ID + "' class='more-link'>(更多&#8230;)</a></p>")
                .replace("{Article_Title}", data[i].Title)
                .replace("{Article_CategoryID}", data[i].CategoryID)
                .replace("{Article_Category}", data[i].Category)
                .replace("{Article_CreationTime}", data[i].CreationTime)
                .replace("{Article_Html}", data[i].Body);
                article = article.replace("{Article_Title_Url}", encodeURIComponent(data[i].Title));
                $("#main").append(article);
                
            }
            page++;
            MusicPlayerEnabled();
            lock = false;
        });
}


function bor(musicSrc) {
    if (autoplay) {
        var bower = window.navigator.userAgent;
        if (bower.indexOf("MSIE 6") != -1 || bower.indexOf("MSIE 7") != -1 || bower.indexOf("MSIE 8") != -1)
        { return "<embed src='" + musicSrc + "' class='muc' autostart=true loop=true hiddle=true>"; }
        if (bower.indexOf("Firefox") != -1)
        { return "<audio src='" + musicSrc + "' class='muc' controls loop autoplay preload><p>您的浏览器版本过低请升级您的浏览器</p></audio>" }
        else { return "<audio src='" + musicSrc + "' class='muc' controls loop autoplay preload><p>您的浏览器版本过低请升级您的浏览器</p></audio>" }
    }
    else {
        var bower = window.navigator.userAgent;
        if (bower.indexOf("MSIE 6") != -1 || bower.indexOf("MSIE 7") != -1 || bower.indexOf("MSIE 8") != -1)
        { return "<embed src='" + musicSrc + "' class='muc' loop=true hiddle=true>"; }
        if (bower.indexOf("Firefox") != -1)
        { return "<audio src='" + musicSrc + "' class='muc' controls loop preload><p>您的浏览器版本过低请升级您的浏览器</p></audio>" }
        else { return "<audio src='" + musicSrc + "' class='muc' controls loop preload><p>您的浏览器版本过低请升级您的浏览器</p></audio>" }
    }
};

function MusicPlayerEnabled()
{
    $(".MusicPlayer").unbind().each(function () {
        var downstr = "<p><a href='" + $(this).attr("url") + "'>右键->目标另存为 下载本音频</a></p>";
        $(this).html(bor($(this).attr("url"))+downstr);
    });
}

$(document).ready(function () {
    blog.init();
    if (typeof (ArticleList) != "undefined")
    {
        LoadArticles();
        $(window).scroll(function () {
            totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
            if ($(document).height() <= totalheight) {
                LoadArticles();
            }
        });
    }
});