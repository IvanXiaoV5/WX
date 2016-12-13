<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="web.gov.main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>信息查询系统</title>
    <link href="../Style/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../Style/bootstrap/js/bootstrap.min.js"></script>
    <style>
        .context{
            padding-top:70px;
            padding-left:10px;
            padding-right:10px;
        }
        .shopList{
            width:30%;
            float:left;
            padding-right:10px;
        }
        .table_list{
            width:70%;
            float:left;
        }
    </style>
</head>

<body>
    <div class="head">
        <nav class="navbar navbar-default navbar-fixed-top" role="navigation">
            <div class="container">
                <ul class="nav nav-pills navbar-btn" role="tablist" style="display: inline-block;">
                    <li role="presentation" class="active"><a href="#">消毒表</a></li>
                    <li role="presentation"><a href="#">进货表</a></li>
                    <li role="presentation"><a href="#">废物处理表</a></li>
                </ul>
                <form class="navbar-form navbar-right" role="search">
                    <div class="input-group">
                        <input type="text" class="form-control" placeholder="Search" />
                        <span class="input-group-btn">
                            <button class="btn btn-default" type="button"><span class="glyphicon glyphicon-search"></span></button>
                        </span>
                    </div>
                </form>
            </div>
        </nav>
    </div>
    <!--导航条-结束-->
    <div class="context">
        <div class="shopList">
            <div class="list-group">
                <a href="#" class="list-group-item active">唐老鸭火锅店  </a>
                <a href="#" class="list-group-item">非诚勿烤-韩国烧烤</a>
                <a href="#" class="list-group-item">老王川菜</a>
                <a href="#" class="list-group-item">老干妈麻辣烫</a>
                <a href="#" class="list-group-item">七天连锁酒店</a>
            </div>
        </div>
        <div class="table_list">
            <div class="table-responsive">
                <table class="table table-hover">
                   <thead>
                       <tr><th>消毒时间</th><th>消毒方式</th><th>物品</th><th>经手人</th></tr>
                   </thead>
                    <tbody>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        <tr>
                            <td>2016年12月8日22:49:24</td><td>消毒柜</td><td>筷子</td><td>王晓霞</td>
                        </tr>
                        
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    
</body>
</html>
