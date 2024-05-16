using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

//Recent Modified : 04-10-2016
namespace TTS.cargomanagement
{
    #region struct for tyre detail and gap details
    struct tyreCoordinate
    {
        public double x, y, z, l, w, h;
        public int qty, orient;
        public Tyre InTyre;
        private string type;
        private string config, tyresize, brand, sidewall, rimsize;
        // properties are used only to show columns in gridview in asp page.
        public string TyreType { get { return type; } set { type = value; } }
        public string Config { get { return config; } set { config = value; } }
        public string Tyresize { get { return tyresize; } set { tyresize = value; } }
        public string Brand { get { return brand; } set { brand = value; } }
        public string Sidewall { get { return sidewall; } set { sidewall = value; } }
        public string Rimsize { get { return rimsize; } set { rimsize = value; } }

        public double X_axis { get { return x; } }
        public double Y_axis { get { return y; } }
        public double Z_axis { get { return z; } }
        public double Length { get { return l; } }
        public double Width { get { return w; } }
        public double Height { get { return h; } }
        public int Quantity { get { return qty; } }
        public int Orient { get { return orient; } }
    }
    struct gapCoordinate
    {
        public double X, Y, Z, l, w, h;
    }
    #endregion
    public partial class loadplanning : System.Web.UI.UserControl
    {
        DataAccess daTTS = new DataAccess(ConfigurationManager.ConnectionStrings["TTSDB"].ConnectionString);
        DataAccess daCOTS = new DataAccess(ConfigurationManager.ConnectionStrings["ORDERDB"].ConnectionString);

        #region common variables and lists
        Container container;
        Dictionary<int, Tyre> indexTyres = new Dictionary<int, Tyre>();
        List<Tyre> Tyres = new List<Tyre>();
        List<tyreCoordinate> lstLoadedTyre = new List<tyreCoordinate>();
        gapCoordinate gW = new gapCoordinate(); //gap width;
        double r_Z, r_Y, r_X;
        double c_Z;
        // when splitting the quantity of tyre to arrange, two set of tyres created one with new id(remaining tyres) and another is fitting quantity(fituid), to find the tyre using the fitUid.
        int newUid = 0;
        int fitUid = 0;

        bool isValidVAlign = true, isValidSAlign = true; // V-Align = vertical align, S-align = slanding align
        //bool isCheckedSland = false;
        //bool isLastRowOfContainer = false;
        double initialHypotesus;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SqlParameter[] sp1 = new SqlParameter[1];
                sp1[0] = new SqlParameter("@UserName", Request.Cookies["TTSUser"].Value);
                DataTable dtUser = (DataTable)daTTS.ExecuteReader_SP("Sp_Sel_UserLevel", sp1, DataAccess.Return_Type.DataTable);
                if (dtUser.Rows.Count > 0)
                {
                    if (dtUser.Rows[0]["cotsexpcargomanagement"].ToString() == "True")
                    {
                        bindddlCustomers();
                    }
                }

            }
        }

        void bindddlCustomers()
        {
            try
            {
                DataTable dtCustList = new DataTable();
                dtCustList = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_User_OrderComplete", DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlCustomer, dtCustList, "custfullname", "CustCode", "CHOOSE");
                if (ddlCustomer.Items.Count == 2)
                {
                    ddlCustomer.SelectedIndex = 1;
                    ddlCustomer_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvLoadedTyreList.DataSource = null;
                gvLoadedTyreList.DataBind();
                if (ddlCustomer.SelectedIndex <= 0) return;
                ddlOrder.Items.Clear();
                SqlParameter[] sp_param1 = new SqlParameter[] { new SqlParameter("@custcode", ddlCustomer.SelectedValue) };
                DataTable dtOrders = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_OrderCompleted", sp_param1, DataAccess.Return_Type.DataTable);
                Utilities.ddl_Binding(ddlOrder, dtOrders, "OrderRefNo", "OrderRefNo", "Choose");
                if (ddlOrder.Items.Count == 2)
                {
                    ddlOrder.SelectedIndex = 1;
                    ddlOrder_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        protected void ddlOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                gvLoadedTyreList.DataSource = null;
                gvLoadedTyreList.DataBind();
                getOrderMasterDetails();
                getContainerDetails();
                getTyreDetails();
                sortByVolume();
                sortNest();
                planTyreOrder();

                // bind gridview to display tyre inside tyres

                if (lstLoadedTyre.Count > 0)
                {
                    List<tyreCoordinate> tmpLst = new List<tyreCoordinate>();
                    tmpLst = lstLoadedTyre.ToList();
                    gvLoadedTyreList.DataSource = lstLoadedTyre;
                    gvLoadedTyreList.DataBind();
                    // convert list to json format and store in localStorage(browser space) for use it in page level for ThreeDRendering
                    var jsonSerializer = new JavaScriptSerializer();
                    string jsonString = jsonSerializer.Serialize(tmpLst);
                    jsonString.Replace('"', '\'');
                    ScriptManager.RegisterStartupScript(Page, GetType(), "StoreTyreObj", "localStorage.setItem(\"tyres_jsonObj\", \'" + jsonString + "\');", true);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowLinkTo3d", "ShowLinkTo3d(1)", true);
                    lblGvHeadMsg.Text = "ALL DIMENSIONS MENTIONED IN MM (MILLIMETER)";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "ShowLinkTo3d", "ShowLinkTo3d(0)", true);
                }

                // bind in gridview to diplay tyre left from loading (not enough space) 

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_LoadPlanning", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        void getOrderMasterDetails()
        {

            SqlParameter[] sp1 = new SqlParameter[2];
            sp1[0] = new SqlParameter("@CustCode", ddlCustomer.SelectedValue);
            sp1[1] = new SqlParameter("@OrderRefNo", ddlOrder.SelectedItem.Text);
            DataTable dt = (DataTable)daCOTS.ExecuteReader_SP("sp_sel_SplIns", sp1, DataAccess.Return_Type.DataTable);
            lblCargoSplIns.Text = dt.Rows[0]["SplIns"].ToString().Replace("~", "\r\n") + "<br/>DATA ENTERED BY : " + dt.Rows[0]["UserName"].ToString() + " - " + dt.Rows[0]["OrderDate"].ToString();
            lblUserDetails.Text = "PLANT : " + dt.Rows[0]["Plant"].ToString();
        }
        void getContainerDetails()
        {
            try
            {
                SqlParameter[] sp_param2 = new SqlParameter[] { 
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue) };
                DataTable dtConDetail = (DataTable)daTTS.ExecuteReader_SP("SP_GET_ContainerDetails", sp_param2, DataAccess.Return_Type.DataTable);
                if (dtConDetail.Rows.Count <= 0) return;
                double conLen, conWidth, conHeight;
                conLen = Convert.ToDouble(dtConDetail.Rows[0]["ContainerLength"]);
                conWidth = Convert.ToDouble(dtConDetail.Rows[0]["containerWidth"]);
                conHeight = Convert.ToDouble(dtConDetail.Rows[0]["ContainerHeight"]);
                container = new Container(len: conLen, width: conWidth, height: conHeight); // here the container details are assigned.
                lblContainerDimension.Text = "<table><tr><th>Width(X)</th><td>" + dtConDetail.Rows[0]["containerWidth"].ToString() + "</td></tr><tr><th>Height(Y)</th><td>" + dtConDetail.Rows[0]["ContainerHeight"].ToString() + "</td></tr><tr><th>Length(Z)</th><td>" + dtConDetail.Rows[0]["ContainerLength"].ToString() + "</td></tr></table>";
                string jsonString = "{ \"Length\":" + (conLen) + ", \"Width\":" + (conWidth) + ", \"Height\":" + conHeight + "}";
                var jsonSerializer = new JavaScriptSerializer();
                jsonString.Replace('"', '\'');
                ScriptManager.RegisterStartupScript(Page, GetType(), "StoreContainerObj", "localStorage.setItem(\"container_jsonObj\", \'" + jsonString + "\');", true);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_LoadPlanning", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        void getTyreDetails()
        {
            try
            {
                lblTotQTy.Text = "";
                SqlParameter[] sp_param3 = new SqlParameter[] { 
                    new SqlParameter("@custcode", ddlCustomer.SelectedValue),
                    new SqlParameter("@orderid", ddlOrder.SelectedValue) };
                DataTable dtOrderItems = (DataTable)daCOTS.ExecuteReader_SP("SP_LST_OrderItemDimension", sp_param3, DataAccess.Return_Type.DataTable);
                if (dtOrderItems.Rows.Count <= 0) return;

                gv_CargoOrderList.DataSource = dtOrderItems;
                gv_CargoOrderList.DataBind();

                object sumQty;
                sumQty = dtOrderItems.Compute("Sum(quantity)", "");
                lblTotQTy.Text = "TOTAL QTY: " + sumQty.ToString();

                int i = 1;
                foreach (DataRow dr in dtOrderItems.Rows)
                {
                    string tyreType = dr["tyretype"].ToString();
                    double od = Convert.ToDouble(dr["OuterDiameter"]);
                    double tw = Convert.ToDouble(dr["treadWidth"]);
                    double sw = Convert.ToDouble(dr["sideWallWidth"]);
                    int qty = Convert.ToInt32(dr["quantity"]);

                    Tyre tyre = new Tyre(tyreType: tyreType, OD: od, TW: tw, SW: sw, unit: DimensionsIn.mm, qty: qty);
                    tyre.Config = dr["Config"].ToString();
                    tyre.Tyresize = dr["tyresize"].ToString();
                    tyre.Brand = dr["brand"].ToString();
                    tyre.Sidewall = dr["sidewall"].ToString();
                    tyre.Rimsize = dr["rimsize"].ToString();
                    tyre.SetU_Id = i;
                    Tyres.Add(tyre);
                    i++;
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_DimensionMaster", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        void sortByVolume()
        {
            Tyres.Sort((a, b) => a.GetEachVolume.CompareTo(b.GetEachVolume));
            Tyres = (Tyres.OrderByDescending(x => x.GetEachVolume).ThenBy(x => x.GetQuantity)).ToList<Tyre>();
            int i = 1;
            indexTyres.Clear();
            foreach (Tyre tyre in Tyres)
            {
                indexTyres.Add(i, tyre);
                i++;
            }
        }
        void sortNest()
        {
            //refer algorithm in Pad( **Container Loading**)
            Tyres = indexTyres.Values.ToList();
            for (int i = 0; i < indexTyres.Values.Count; i++)
            {
                for (int j = i + 1; j < indexTyres.Values.Count; j++)
                {
                    if (Tyres[i].HasRim == false)
                    {
                        if (Tyres[i].InTyreId.Equals(0) && Tyres[i].OutTyreId.Equals(0))
                        {
                            // if j^th tyre alread compared then inTyreId or OutTyreId has the value of fit tyre ID. 
                            //if it's null compare else continue with next Tyre in the list
                            if (Tyres[j].InTyreId.Equals(0) && Tyres[j].OutTyreId.Equals(0))
                            {
                                if (Tyres[i].GetInnerDia > Tyres[j].GetOuterDia + 2)
                                {
                                    if (Tyres[i].GetTreadWidth >= Tyres[j].GetTreadWidth)
                                    {
                                        int outQty, inQty, remQty;
                                        outQty = Tyres[i].GetQuantity;
                                        inQty = Tyres[j].GetQuantity;
                                        if (outQty > inQty)
                                        {
                                            remQty = outQty - inQty;
                                            Tyres[i].SetQuantity = outQty - remQty;
                                            Tyres[i].InTyreId = Tyres[j].GetU_Id;
                                            Tyres[j].OutTyreId = Tyres[i].GetU_Id;
                                            addSplittedTyres(Tyres[i], remQty);
                                        }
                                        else if (outQty < inQty)
                                        {
                                            remQty = inQty - outQty;
                                            Tyres[j].SetQuantity = inQty - remQty;
                                            Tyres[i].InTyreId = Tyres[j].GetU_Id;
                                            Tyres[j].OutTyreId = Tyres[i].GetU_Id;
                                            addSplittedTyres(Tyres[j], remQty);
                                        }
                                        else
                                        {
                                            Tyres[i].InTyreId = Tyres[j].GetU_Id;
                                            Tyres[j].OutTyreId = Tyres[i].GetU_Id;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

        }

        void planTyreOrder()
        {
            try
            {
                r_Z = container.GetLength;
                r_Y = container.GetHeight;
                r_X = container.GetWidth;
                gW.X = 0; gW.Y = 0; gW.Z = 0;
                gW.l = container.GetLength; gW.w = container.GetWidth; gW.h = container.GetHeight;
                int id, orient;
                Tyre tyre;

                do
                {
                    if (if_S_TyreFit(out id, out orient, out tyre) == true)
                    {
                        double tyreTWidth = tyre.GetQuantity * tyre.GetWidth;
                        int j = 1;
                        foreach (Tyre rTyre in indexTyres.Values)
                        {
                            if (rTyre.GetU_Id == id)
                            {
                                tyre = rTyre;
                                break;
                            }
                            j++;
                        }
                        if (orient == 3)
                        {
                            if (tyreTWidth <= gW.h)
                            {
                                loadTyre(tyre, orient);
                                indexTyres.Remove(j);
                                Tyres = indexTyres.Values.ToList();
                                sortByVolume();
                                r_Y -= tyreTWidth;
                            }
                            else
                            {
                                splitTyreQty(j, tyre, gW, orient);
                                continue;
                            }
                        }
                        else if (orient == 2)
                        {
                            if (tyreTWidth <= gW.w)
                            {
                                loadTyre(tyre, orient);
                                indexTyres.Remove(j);
                                Tyres = indexTyres.Values.ToList();
                                sortByVolume();
                                r_Y -= tyre.GetOuterDia;
                            }
                            else
                            {
                                splitTyreQty(j, tyre, gW, orient);
                                continue;
                            }
                        }
                        else if (orient == 1)
                        {
                            if (tyreTWidth <= gW.l)
                            {
                                loadTyre(tyre, orient);
                                indexTyres.Remove(j);
                                Tyres = indexTyres.Values.ToList();
                                sortByVolume();
                                r_Y -= tyre.GetOuterDia;
                            }
                            else
                            {
                                splitTyreQty(j, tyre, gW, orient);
                                continue;
                            }
                        }
                        else if (orient == 4)
                        {
                            initialHypotesus = getHypotesus(gW.w, tyre.GetOuterDia);
                            if (tyreTWidth + initialHypotesus <= gW.h)
                            {
                                loadTyre(tyre, orient);
                                indexTyres.Remove(j);
                                Tyres = indexTyres.Values.ToList();
                                sortByVolume();
                                r_Y -= tyreTWidth + initialHypotesus;
                            }
                            else
                            {
                                splitTyreQty(j, tyre, gW, orient);
                                continue;
                            }
                        }
                    }
                    else
                    {
                        bool checkX = false, checkZ = false;
                        if (r_Y > 0)
                        {
                            gW.Y = (container.GetHeight - r_Y);
                            if (if_S_TyreFit(out id, out orient, out tyre) == true) continue;
                            else checkX = true;
                        }
                        else if (r_Y == 0) checkX = true;

                        if (r_X > 0 && checkX == true)
                        {
                            gW.X = (container.GetWidth - r_X);
                            gW.Y = 0;
                            // to be verify 
                            gW.w = r_X;
                            gW.h = container.GetHeight;
                            r_Y = container.GetHeight;
                            if (if_S_TyreFit(out id, out orient, out tyre) == true) continue;
                            else checkZ = true;
                        }
                        else if (r_X == 0) checkZ = true;
                        if (r_Z > 0 && checkZ == true)
                        {
                            gW.X = 0;
                            gW.Y = 0;
                            gW.Z = (container.GetLength - r_Z);
                            gW.w = container.GetWidth;
                            gW.h = container.GetHeight;
                            gW.l = c_Z;
                            r_X = container.GetWidth;
                            r_Y = container.GetHeight;
                            //if (isLastRowOfContainer == false)
                            // checkIsLastRow();
                            if (if_S_TyreFit(out id, out orient, out tyre) == true) continue;
                            else break;
                        }
                        else break;
                    }
                } while (indexTyres.Values.Count > 0);
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_LoadPlanning", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        void loadTyre(Tyre tyre, int orient) // load the planned tyre in the list 
        {
            try
            {
                double totWidth = ((tyre.GetWidth + tyre.WrapWidth) * tyre.GetQuantity);
                tyreCoordinate structTyre = new tyreCoordinate();
                structTyre.x = gW.X;
                structTyre.y = gW.Y;
                structTyre.z = gW.Z;
                structTyre.qty = tyre.GetQuantity;
                structTyre.TyreType = tyre.GetTyreType;
                structTyre.Config = tyre.Config;
                structTyre.Tyresize = tyre.Tyresize;
                structTyre.Brand = tyre.Brand;
                structTyre.Sidewall = tyre.Sidewall;
                structTyre.Rimsize = tyre.Rimsize;

                if (orient == 1)
                {
                    structTyre.w = tyre.GetOuterDia;
                    structTyre.h = tyre.GetOuterDia;
                    structTyre.l = totWidth;
                    structTyre.orient = 1;
                    // to get the dimension of inner gap
                    gW.h -= tyre.GetOuterDia;
                    if (container.GetHeight - r_Y == 0)
                    {
                        gW.l = totWidth;
                        gW.w = tyre.GetOuterDia;
                        c_Z = r_Z;
                        if (r_X - container.GetWidth == 0 && r_Y - container.GetHeight == 0) r_Z -= totWidth;
                        r_X -= tyre.GetOuterDia;
                    }
                    gW.Y += tyre.GetOuterDia;
                }
                else if (orient == 2)
                {
                    structTyre.l = tyre.GetOuterDia;
                    structTyre.h = tyre.GetOuterDia;
                    structTyre.w = totWidth;
                    structTyre.orient = 2;
                    // to get the dimension of inner gap
                    gW.h -= tyre.GetOuterDia;
                    if (container.GetHeight - r_Y == 0)
                    {
                        gW.l = tyre.GetOuterDia;
                        gW.w = totWidth;
                        c_Z = r_Z;
                        if (r_X - container.GetWidth == 0 && r_Y - container.GetHeight == 0) r_Z -= tyre.GetOuterDia;
                        r_X -= totWidth;
                    }
                    gW.Y += tyre.GetOuterDia;
                }
                else if (orient == 3)
                {
                    structTyre.w = tyre.GetOuterDia;
                    structTyre.l = tyre.GetOuterDia;
                    structTyre.h = totWidth;
                    structTyre.orient = 3;
                    // to get the dimension of inner gap
                    gW.h -= totWidth;
                    if (container.GetHeight - r_Y == 0)
                    {
                        gW.l = tyre.GetOuterDia;
                        gW.w = tyre.GetOuterDia;
                        c_Z = r_Z;
                        if (r_X - container.GetWidth == 0 && r_Y - container.GetHeight == 0) r_Z -= tyre.GetOuterDia;
                        r_X -= tyre.GetOuterDia;
                    }
                    gW.Y += totWidth;
                }
                else if (orient == 4)
                {
                    structTyre.w = tyre.GetOuterDia;
                    structTyre.l = tyre.GetOuterDia;
                    structTyre.h = totWidth;
                    structTyre.orient = 4;

                    gW.h -= (totWidth + initialHypotesus);
                    if (container.GetHeight - r_Y == 0)
                    {
                        gW.l = tyre.GetOuterDia;
                        gW.w = tyre.GetOuterDia;
                        c_Z = r_Z;
                        if (r_X - container.GetWidth == 0 && r_Y - container.GetHeight == 0) r_Z -= tyre.GetOuterDia;
                        r_X -= tyre.GetOuterDia;
                    }
                    gW.Y += totWidth + initialHypotesus;
                }
                if (!tyre.InTyreId.Equals(0)) // to load the tyre inside the tyre 
                {
                    foreach (Tyre t in indexTyres.Values)
                    {
                        if (t.GetU_Id == tyre.InTyreId)
                        {
                            structTyre.InTyre = t;
                            break;
                        }
                    }
                }
                lstLoadedTyre.Add(structTyre);
                fitUid = 0;
            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_LoadPlanning", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }
        bool splitTyreQty(int index, Tyre tyre, gapCoordinate gC, int orient) // 
        {
            int tyreQty = 0, totQty = tyre.GetQuantity;
            indexTyres.Remove(index);
            if (newUid == 0) newUid = (int)indexTyres.Values.Select(x => x.GetU_Id).Max() + 1;
            else newUid += 1;
            if (orient == 1) tyreQty = (int)System.Math.Floor((gC.l / (tyre.GetWidth + tyre.WrapWidth)));
            else if (orient == 2) tyreQty = (int)System.Math.Floor((gC.w / (tyre.GetWidth + tyre.WrapWidth)));
            else if (orient == 3) tyreQty = (int)System.Math.Floor((gC.h / (tyre.GetWidth + tyre.WrapWidth)));
            else if(orient == 4) tyreQty = (int)System.Math.Floor((gC.h - initialHypotesus / (tyre.GetWidth + tyre.WrapWidth)));
            //tyreQty = (int)System.Math.Floor((gC.h / tyre.GetWidth));
            if (tyreQty <= 0) return false;
            Tyres = indexTyres.Values.ToList<Tyre>();
            tyre.SetQuantity = tyreQty;
            Tyres.Add(tyre);
            fitUid = tyre.GetU_Id;
            Tyre remTyre = new Tyre(tyre);
            remTyre.SetQuantity = totQty - tyreQty;
            remTyre.SetU_Id = newUid;
            Tyres.Add(remTyre);
            sortByVolume();
            return true;
        }
        void addSplittedTyres(Tyre tyre, int qty) // used when plan for fit tyre inside tyre
        {
            Tyre t = new Tyre(tyre);
            t.SetU_Id = Tyres.Select(x => x.GetU_Id).Max() + 1;
            t.SetQuantity = qty;
            Tyres.Add(t);
            sortByVolume();
        }
        //  Check if small Tyre fit in the space. if fit then get the tyre, id, and its orientation. else return false
        bool if_S_TyreFit(out int uid, out int orient, out Tyre t)
        {
            foreach (Tyre tyre in indexTyres.Values)
            {
                if (!tyre.OutTyreId.Equals(0)) continue;
                if (fitUid > 0 && tyre.GetU_Id != fitUid) continue;
                
                if (tyre.GetOuterDia <= gW.w && tyre.GetOuterDia <= gW.l && (tyre.GetWidth + tyre.WrapWidth) <= gW.h)
                {
                    orient = 3;
                    uid = tyre.GetU_Id;
                    t = tyre;
                    return true;
                }
            }
            if (isValidSAlign == true && if_Sland_TyreFit(out uid, out orient, out t) == true)
            {
                return true;
            }
            else if (isValidVAlign == true)
            {
                foreach (Tyre tyre in indexTyres.Values)
                {
                    if (tyre.GetOuterDia <= gW.h && tyre.GetOuterDia <= gW.l && (tyre.GetWidth + tyre.WrapWidth) <= gW.w)
                    {
                        orient = 2;
                        uid = tyre.GetU_Id;
                        t = tyre;
                        return true;
                    }
                    else if (tyre.GetOuterDia <= gW.w && tyre.GetOuterDia <= gW.h && (tyre.GetWidth + tyre.WrapWidth) <= gW.l)
                    {
                        orient = 1;
                        uid = tyre.GetU_Id;
                        t = tyre;
                        return true;
                    }
                }
            }
            uid = 0;
            orient = 0;
            t = null;
            return false;
        }
        bool if_Sland_TyreFit(out int uid, out int orient, out Tyre t)
        {
            uid = 0;
            orient = 0;
            t = null;
            bool status = false;
            foreach (Tyre tyre in indexTyres.Values)
            {
                double tempHypo = getHypotesus(gW.w, tyre.GetOuterDia) + tyre.GetTreadWidth;
                if (tyre.GetOuterDia >= gW.w && tyre.GetOuterDia <= gW.w + 50 && gW.h >= tempHypo && gW.l >= tyre.GetOuterDia ) // 50 - approximate for 2 inch
                {
                    uid = tyre.GetU_Id;
                    orient = 4;
                    t = tyre;
                    status = true;
                    break;
                }
            }
           return status;
        }
        protected void gvLoadedTyreList_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Adding a column manually once the header created
                    if (e.Row.RowType == DataControlRowType.Header) // If header created
                    {
                        GridView ProductGrid = (GridView)sender;
                        GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                        TableCell HeaderCell = new TableCell();
                        HeaderCell.Text = "S No";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "PLATFORM";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "BRAND";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "SIDEWALL";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "TYRE TYPE";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "TYRE SIZE";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "RIM";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "CONTAINER";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                        HeaderCell.Style.Add("background-color", "#36b4ae;");
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "TYRE";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = 3; // For merging three columns (Direct, Referral, Total)
                        HeaderCell.Style.Add("background-color", "#36b4ae;");
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "QTY";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        HeaderCell = new TableCell();
                        HeaderCell.Text = "ORIENT";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.RowSpan = 2;
                        HeaderRow.Cells.Add(HeaderCell);

                        //Adding the Row at the 0th position (first row) in the Grid
                        ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                    }
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    if (e.Row.DataItem != null)
                    {
                        tyreCoordinate tc = (tyreCoordinate)e.Row.DataItem;
                        if (tc.InTyre != null)
                        {
                            Tyre t = tc.InTyre;
                            tyreCoordinate newtc = new tyreCoordinate();
                            newtc.Config = t.Config;
                            newtc.Brand = t.Brand;
                            newtc.Sidewall = t.Sidewall;
                            newtc.TyreType = t.GetTyreType;
                            newtc.Tyresize = t.Tyresize;
                            newtc.Rimsize = t.Rimsize;
                            newtc.qty = t.GetQuantity;
                            tc.InTyre = null;
                            e.Row.DataItem = tc;
                            List<tyreCoordinate> tc1 = (List<tyreCoordinate>)gvLoadedTyreList.DataSource;
                            tyreCoordinate oldtc = tc1[0];
                            oldtc.InTyre = null;
                            tc1.RemoveAt(e.Row.RowIndex);
                            tc1.Insert(e.Row.RowIndex, oldtc);
                            tc1.Insert(e.Row.RowIndex + 1, newtc);
                            gvLoadedTyreList.DataSource = tc1;
                            gvLoadedTyreList.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.ToString();
            }
        }
        protected void gvLoadedTyreList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                    e.Row.Cells[2].Visible = false;
                    e.Row.Cells[3].Visible = false;
                    e.Row.Cells[4].Visible = false;
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                    e.Row.Cells[13].Visible = false;
                    e.Row.Cells[14].Visible = false;
                }

            }
            catch (Exception ex)
            {
                Utilities.WriteToErrorLog("CARGO_MANAGEMENT_LoadPlanning", Request.Url.ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, 1, ex.Message);
            }
        }

        double getHypotesus(double adjacent, double diagonal)
        {
            return Math.Sqrt( Math.Pow(adjacent,2) + Math.Pow(diagonal,2));
        }
    }
}