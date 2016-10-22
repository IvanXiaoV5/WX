<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="web.Config" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>微信公众号配置</title>
    <style>
        textarea{
            width:100%;
            height:400px;
        }
    </style>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script>
        $(function () {
            $("#setMenu").click(function () {
                submitdata();
            });
        });


        function submitdata() {
            var txt = $("#menudata").val();
            $.ajax({
                type: "post",
                data: "menudata="+txt,
                url: "test/Webserver.ashx?method=pushMenu",
                success: function (data) {
                    $("#menudata").val(data)
                }
            });
        }
    </script>
</head>
<body>
    <div>
        <div class="left">
            <input type="button" value="修改菜单" id="setMenu" /><span id="info"></span>
        </div>
        <div class="right">
            <textarea id="menudata" ></textarea>
        </div>
    </div>
</body>
</html>
