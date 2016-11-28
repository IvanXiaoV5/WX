<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reg.aspx.cs" Inherits="web.gov.reg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title><%=ConfigurationManager.AppSettings["SystemName"]+"" %></title>
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery.validate.min.js"></script>
    <script src="../js/messages_zh.js"></script>
    <script src="../Style/bootstrap/js/bootstrap.min.js"></script>
    <link href="../Style/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/css.css" rel="stylesheet" />
    <script>

        $().ready(function () {
            var validator= $("#reg").validate(
                {
                    errorLabelContainer: $("#errorinfo")
                }
                
                );
           
        });

        function showerror() {
            // $("#errorinfo").html(error);
            alert("111");
        }
        
    </script>
</head>
<body>
    <div class="jumbotron">
            <h1><%=ConfigurationManager.AppSettings["SystemName"]+"" %></h1>
            <p>用信息化提升办事效率!<a class="btn btn-primary btn-sm" href="#" role="button">登 录</a></p>
            <p></p>
        </div>
    <div class="container-fluid">
        


        <!--注册表格-->
        <div class="form">
            <div class="page-header">
                <h1>管理单位注册 <small>请认真填写相关信息</small></h1>
            </div>
            <div id="errorinfo" class="alert alert-danger errorinfo" role="alert"></div>
            <form id="reg" class="reg" method="post" action="www.qq.com">
                <div class="input-group">
                    <span class="input-group-addon"><span class="glyphicon glyphicon-user"></span></span>
                    <input type="text" name="username" class="form-control" placeholder="用户名" required  maxlength="64"/>
                </div>
                <div class="input-group">
                    <span class="input-group-addon"><span class="glyphicon glyphicon-info-sign"></span></span>
                    <input type="password" name="passowrd" class="form-control" placeholder="密 码" minlength="6" maxlength="64" required/>
                </div>
                <div class="input-group">
                    <span class="input-group-addon"><span class="glyphicon glyphicon-tag"></span></span>
                    <input type="text" name="govname" class="form-control" placeholder="单位名称" required maxlength="64"/>
                </div>
                <div class="input-group">
                    <span class="input-group-addon"><span class="glyphicon glyphicon-globe"></span></span>
                    <input type="text" name="address" class="form-control" placeholder="单位地址" required maxlength="256" />
                </div>
                
                <input type="button" class="btn btn-primary btn-lg btn-block" value="注 册" />
            </form>

        </div>
    </div>
    
    
    
</body>
</html>
