<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CaptchaControl.ascx.cs" Inherits="NicePhotos.Controls.CaptchaControl" %>
<table id="captchaContainer" style="margin-left:auto;margin-right:auto;" runat="server">
    <tr>
        <td style="text-align:center">
            <asp:Image ID="imgCaptch" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="text-align:right;">
            <asp:ImageButton ID="imgRefresh" AlternateText="See Anotherone" runat="server"
                
                ImageUrl="data:image/gif;base64,R0lGODlhFAAUAOZSAFlZWV9fX5OTk1ZWVl1dXWpqamhoaFpaWlhYWGNjY2RkZHFxcVNTU2xsbNra2mJiYlJSUmBgYH19fbu7u7i4uGFhYZ+fn87OztXV1Y+Pj29vb7GxsVdXV5aWllRUVGVlZV5eXr29vVxcXM/Pz9fX18fHx6urq5ycnEZGRre3t8bGxk9PT0xMTG5ubnx8fIqKipCQkJ2dnczMzMXFxWlpaVFRUdnZ2Zubm7S0tL6+vm1tbXV1dZGRkYWFha+vr4mJidzc3Hd3d4aGhsDAwH5+fnt7e8LCwrOzs3BwcLKysrq6unZ2dpiYmHNzc3p6esrKyoGBgVtbW////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAEAAFIALAAAAAAUABQAAAfTgFKCg4RSJQ6FiYoNERuKjykdNAMALiGPgyYaBwcgBQYQCDCYAgAIAw8KCgEIBTiPMSIeBEE/BQwVFpgzClEGAoJCRReCFAsnhQI1CRKDNoMjAxAthQUED0aPSFFRT4QGUQ2YL1EAFIMOHwA6mDwcBD6ERFEJE48aHFEqhB0oLDeDgAza8ICAgUIhFgATxKSJDClKAATwoOvREScrAGSAEiABgh2PMCwBES5BgAMAACzA8GiCiAEBPvQ6EGEhJgcZAuDqkYQEpkIXJDAY8vNRjkeBAAA7" onclick="imgRefresh_Click" 
                />
            <asp:ImageButton ID="imbListen" AlternateText="Listen" runat="server" 
                ImageUrl="data:;base64,iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAYAAACNiR0NAAAABmJLR0QA/wD/AP+gvaeTAAAAB3RJTUUH1wsIFxkDauvSngAAAU9JREFUOI3t071KnUEQxvGfevxIIKggahQipPAjcmzUUrQLBC8hViJJK9h4EbmBFHoB6YOIFgELIRALEREEKy0MQpIiojE5FjvCcnhPPNoJPjDMy+zsf2d35uVRNfQOFXRXxXvwFQN3gS0HrIJyFm9FCWfYqAfUjBX8qwGsxGGTkTOTb25FZ2YD2MRVBrsBPo/KlvAbXVjDpxxWKbDLglgZp/iIlvhexBx+xa201wAWWRkLUdkzrOIzXsb6eGM9j1mldTzBCA4wiONY678P8Gf4dpzjKS6iwrb7AEfDH6IXJ1JjGnBWwt8s+U/4pvBFBy5gB0eYxjZeRYV7DZE0i75s0wTeSl0rZfExvMau1OFvAX0TNva/q43Hpnx88sH+gi2pQd+lubxVL7BfA/geQ/iAH+ioB0jq5mEAh6vW+qS/ab5e2I2apTcq0pTU4Qeia7anWP41cTR8AAAAAElFTkSuQmCC" 
                onclick="imbListen_Click" />
        </td>
    </tr>
</table>