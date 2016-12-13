<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reguser.aspx.cs" Inherits="web.shop.reguser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <title>用户注册</title>

    <link href="../Style/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../Style/bootstrap/js/bootstrap.min.js"></script>
         <link type="text/css" href="https://res.wx.qq.com/open/libs/weui/1.1.0/weui.min.css" rel="stylesheet"/>
</head>
<body>
    <div class="page home js_show">
        <div class="page-header">
            <h1>用户注册 <small>请认真填写相关信息</small></h1>
        </div>

        <div>
            <div class="weui-cells__title">真实名字</div>
            <div class="weui-cells">
                <div class="weui-cell">
                    <div class="weui-cell__bd">
                        <input class="weui-input" type="text" placeholder="请输入你的真实名字，以后不能修改。" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
