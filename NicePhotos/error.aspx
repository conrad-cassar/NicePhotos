<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="NicePhotos.error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ooops!</title>

    <style type="text/css">
        body, form, html
        {
            width:100%;
            height:100%;
            margin:0px;
            padding:0px;
            border:none;
            background-color:#fafafa;
        }
        #errorContainer
        {
            margin:0px auto 0px auto;
            padding:10px;
            padding-top:10%;
            color:#999;
            font-family:Calibri;
            font-size:20px;
        }
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:auto;height:auto;">
        <table id="errorContainer">
            <tr>
                <td>
                    <img src="/Images/errorCloud.png" alt="" width="80px" height="50px" />
                </td>
                <td colspan="2" style="font-size:2.5em;color:#990033 !important;">
                    Ooops...
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:1.3em;">
                    Seems that we had a problem :[<br /><br />
                    <i>Please try again later,<br />or call <b>Chuck Norris</b> to fix everything!</i>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
