<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log.aspx.cs" Inherits="web.log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Button ID="btn_dellog" runat="server" Text="删除今天日志" OnClick="btn_dellog_Click" />
        
    <div>
    <%=ViewState["log"] %>
    </div>
    </form>
</body>
</html>
