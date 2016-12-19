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
    <style>
        body{
            background-color:#f8f8f8;
        }
        .btn_reg{
            padding-top:10px;
        }
        .page-header{
            padding-left:5px;
        }
        #shop_name{
            display:none;
        }
        #gov_num{
            display:none;
        }
    </style>
</head>
<body>
    <div class="page home js_show">
        <div class="page-header">
            <h2>用户注册 <small></small></h2>
        </div>


        <form method="post" id="reg">
            <div id="shop_num">
                <div class="weui-cells__title">
                    <span>店铺编号:  </span>

                </div>
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="number" placeholder="请输入你所在店铺的编号" />
                        </div>
                    </div>
                </div>
            </div>

            <div>
                <div class="weui-cells__title">真实名字:</div>
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" placeholder="请输入你的真实名字，以后不能修改。" />
                        </div>
                    </div>
                </div>
            </div>


            <div id="shop_name">
                <div class="weui-cells__title">店铺名称:</div>
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="text" placeholder="店铺全名" />
                        </div>
                    </div>
                </div>
            </div>

            <div id="gov_num">
                <div class="weui-cells__title">食品药品监督管理局号码:</div>
                <div class="weui-cells">
                    <div class="weui-cell">
                        <div class="weui-cell__bd">
                            <input class="weui-input" type="number" placeholder="只能输入数字" />
                        </div>
                    </div>
                </div>
            </div>
        </form>

        <div class="btn_reg">
            <a href="javascript:;" class="weui-btn weui-btn_primary">注 册</a>
            <a href="#" onclick="ShopReg()"  id="btn_shopreg" class="weui-btn weui-btn_default">没有店铺号码!</a>
        </div>
        
    </div>
    <script>
        //$(function () {
        //    $("#btn_shopreg").click(function () {
        //        $("#shop_num").css("display", "none");
        //        $("#shop_name").css("display", "block");
        //        $("#gov_num").css("display", "block");
        //    });
        //});


        function ShopReg()
        {
            $("#shop_num").css("display", "none");
            $("#shop_name").css("display", "block");
            $("#gov_num").css("display", "block");
        }
    </script>
</body>
</html>
