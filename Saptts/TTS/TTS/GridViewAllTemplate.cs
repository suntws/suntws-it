using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//A customized class for displaying the Template Column
public class GridViewAllTemplate : ITemplate
{
    //A variable to hold the type of ListItemType.
    ListItemType _templateType;
    //A variable to hold the column name.
    string _columnName;
    //string _typedesc;

    //Constructor where we define the template type and column name.
    public GridViewAllTemplate(ListItemType type, string colname)//, string strTypeDesc
    {
        _templateType = type;
        _columnName = colname;
        //_typedesc = strTypeDesc;
    }

    void ITemplate.InstantiateIn(System.Web.UI.Control container)
    {
        switch (_templateType)
        {
            case ListItemType.Header:
                Label lbl = new Label();

                string[] strHead = _columnName.Split('_');
                if (strHead.Length == 4)
                {
                    //lbl.Text = strHead[3].ToString() + " - BASIC UNIT PRICE<br/><span style='font-size:9px;'>" + _typedesc + "</span>";
                    lbl.Text = strHead[3].ToString();//"<span style='font-size:11px;word-break:break-word;'>" + _typedesc + "</span><br/>" +
                    lbl.CssClass = "exacttypewidth";
                }
                else
                {
                    if (_columnName == "TyreSize")
                        lbl.Text = "TYRE SIZE";
                    else if (_columnName == "RimSize")
                        lbl.Text = "RIM";
                    else if (_columnName == "TotFinishedWT")
                        lbl.Text = "TOT WT Kg";
                    else if (_columnName == "TotPcs")
                        lbl.Text = "TOT QTY";
                    else
                        lbl.Text = _columnName;
                    lbl.CssClass = "";
                }
                container.Controls.Add(lbl);
                break;

            case ListItemType.Item:
                if (_columnName == "TyreSize" || _columnName == "RimSize" || _columnName == "TotFinishedWT" || _columnName == "TotPcs")
                {
                    Label lb1 = new Label();
                    lb1.DataBinding += new EventHandler(lbl_DataBinding);
                    lb1.ID = "lbl_" + _columnName;
                    lb1.CssClass = "css" + _columnName;
                    if (_columnName == "TotFinishedWT" || _columnName == "TotPcs")
                        lb1.Font.Bold = true;
                    container.Controls.Add(lb1);
                }
                else
                {
                    TextBox tb1 = new TextBox();
                    tb1.Width = 50;
                    tb1.Height = 15;
                    tb1.Style.Add("border", "1px solid #000");
                    tb1.Style.Add("font-weight", "bold");
                    tb1.CssClass = "css" + _columnName;
                    tb1.ID = "txt_" + _columnName;
                    tb1.Text = "";
                    tb1.MaxLength = 6;
                    tb1.Attributes.Add("onkeypress", "return isNumberKey(event)");
                    tb1.DataBinding += new EventHandler(txt_DataBinding);
                    container.Controls.Add(tb1);

                    //Label lb1 = new Label();
                    //lb1.DataBinding += new EventHandler(lbl_DataBinding);
                    //container.Controls.Add(lb1);
                }
                break;
        }
    }

    /// <summary>
    /// This is the event, which will be raised when the binding happens.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void lbl_DataBinding(object sender, EventArgs e)
    {
        Label lbldata = (Label)sender;
        GridViewRow container = (GridViewRow)lbldata.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);
        if (dataValue != DBNull.Value)
        {
            lbldata.Text = dataValue.ToString();
        }
    }

    void txt_DataBinding(object sender, EventArgs e)
    {
        TextBox txtdata = (TextBox)sender;
        GridViewRow container = (GridViewRow)txtdata.NamingContainer;
        object dataValue = DataBinder.Eval(container.DataItem, _columnName);
        if (dataValue != DBNull.Value)
        {
            txtdata.Text = dataValue.ToString();
            txtdata.Enabled = false;
            if (dataValue.ToString() == "P-NA" || dataValue.ToString() == "F-NA")
                txtdata.Font.Size = 6;
            else
            {
                txtdata.Font.Size = 8;
                txtdata.Style.Add("background-color", "#ceffba");
            }
        }
    }
}