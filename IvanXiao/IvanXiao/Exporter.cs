using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Web;
using System.Data.OleDb;
using System.Configuration;




namespace IvanXiao
{
    /// <summary>
    ///Exporter 的摘要说明
    /// </summary>
    public static class Exporter
    {
        /// <summary>
        /// 将json转换为DataTable
        /// </summary>
        /// <param name="strJson">得到的json</param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string strJson)
        {
            //转换json格式
            strJson = strJson.Replace("\\", "").Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名   
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');

                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');

                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }


        public static bool ExportToExcel(DataTable dt, String exportName)
        {
            FileInfo fi = null;
            if (dt != null)
            {

                fi = CreateExcel(dt);//在服务器上生成Excel文件
                return ExportExcel(fi, exportName);//下载Excel文件
            }
            return false;
        }

        /// <summary>
        /// 下载压缩文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="xlsName"></param>
        /// <param name="zipName"></param>
        /// <returns></returns>
        public static bool Export(DataTable dt, String xlsName, string zipName)
        {
            FileInfo fi = null;
            if (dt != null)
            {

                fi = CreateExcel(dt);//在服务器上生成Excel文件
                //把XLS文件复制到压缩文件夹
                File.Copy(HttpContext.Current.Server.MapPath("~/") + "stemp.xls", HttpContext.Current.Server.MapPath("~/") + "zipDir\\" + xlsName + ".xls");
                ZipHelper zip = new ZipHelper();
                zip.CompressDirectory(HttpContext.Current.Server.MapPath("~/") + "zipDir", HttpContext.Current.Server.MapPath("~/") + "zipDir.zip", 9, 1024);
                //return ExportExcel(fi, xlsName);//下载Zip文件
                if (Download(HttpContext.Current.Server.MapPath("~/") + "zipDir.zip", zipName + ".zip"))
                {
                    File.Delete(HttpContext.Current.Server.MapPath("~/") + "zipDir.zip");
                    fi.Delete();
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 下载服务器上面的文件
        /// </summary>
        /// <param name="path">文件的地址</param>
        /// <param name="exportName">导出的文件名（需要带扩展名）</param>
        /// <returns></returns>
        private static bool Download(string path, string exportName)
        {
            HttpResponse contextResponse = HttpContext.Current.Response;
            string filename = HttpUtility.UrlEncode(exportName, System.Text.Encoding.UTF8);
            contextResponse.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            contextResponse.AppendHeader("Content-Disposition", "attachment;filename=" + filename);

            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, System.IO.FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    int size = 1024;
                    for (int i = 0; i < fs.Length / size + 1; i++)
                    {
                        byte[] buffer = new byte[size];
                        int lenght = fs.Read(buffer, 0, size);
                        contextResponse.OutputStream.Write(buffer, 0, lenght);
                    }
                }
                contextResponse.Flush();
                return true;
            }

            return false;

        }

        private static bool ExportExcel(FileInfo fi, string exportName)
        {
            if (fi != null)
            {
                try
                {
                    HttpResponse contextResponse = HttpContext.Current.Response;
                    string filename = HttpUtility.UrlEncode(exportName, System.Text.Encoding.UTF8);
                    contextResponse.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                    contextResponse.AppendHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
                    if (fi.Length > 0)
                    {
                        using (FileStream sr = new FileStream(fi.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                        {
                            int size = 1024;//设置每次读取长度。
                            for (int i = 0; i < fi.Length / size + 1; i++)
                            {
                                byte[] buffer = new byte[size];
                                int length = sr.Read(buffer, 0, size);
                                contextResponse.OutputStream.Write(buffer, 0, length);
                            }
                        }
                    }
                    else
                    {
                        contextResponse.WriteFile(fi.FullName);
                    }
                    contextResponse.Flush();
                    fi.Delete();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public static String CreateExcelandSrc(DataTable dt, string name)
        {
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            //  string FileName = "Report_" + HttpContext.Current.Session.SessionID;
            string file = HttpContext.Current.Server.MapPath("~/") + name + ".xls";
            if (File.Exists(file))
            {
                File.Delete(file);//删除该文件}else{MessageBox.Show("不存在文件");
            }
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=Excel 8.0";
            OleDbConnection myConn = new OleDbConnection(strCon);
            try
            {
                string columnname = "";
                string cols = "";

                //获得表结构
                DataTable ndt = new DataTable();
                //清空数据
                //ndt.Rows.Clear();
                ndt.TableName = dt.TableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnname = dt.Columns[i].ColumnName;
                    columnname = ConfigurationManager.AppSettings[columnname] != null ? ConfigurationManager.AppSettings[columnname] : columnname;

                    ndt.Columns.Add(columnname, typeof(string));
                    if (cols == "")
                        cols = "[" + columnname + "] Text";
                    else
                        cols += ",[" + columnname + "] Text";
                }
                OleDbCommand com = new OleDbCommand();
                com.Connection = myConn;
                myConn.Open();
                com.CommandText = "CREATE TABLE [Report](" + cols + ")";
                com.ExecuteNonQuery();
                string strCom = "SELECT * FROM [Report$]";
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。   
                //获取insert语句中保留字符（起始位置）  
                builder.QuotePrefix = "[";
                //获取insert语句中保留字符（结束位置）   
                builder.QuoteSuffix = "]";

                //myCommand.Fill(newds, TableName);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //在这里不能使用ImportRow方法将一行导入到news中，
                    //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                    //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added     
                    DataRow row = ndt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row[j] = dt.Rows[i][j];
                    }
                    ndt.Rows.Add(row);
                }
                //插入数据
                myCommand.Update(ndt);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("<script>alert('create excel failture !" + e.Message + "')</script>");
            }
            finally
            {
                myConn.Close();
            }
            return name + ".xls";
        }



        public static FileInfo CreateExcel(DataTable dt)
        {
            if (dt.Rows.Count <= 0)
            {
                return null;
            }
            //  string FileName = "Report_" + HttpContext.Current.Session.SessionID;
            string file = HttpContext.Current.Server.MapPath("~/") + "stemp.xls";
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=Excel 8.0";
            OleDbConnection myConn = new OleDbConnection(strCon);
            try
            {
                string columnname = "";
                string cols = "";
                //获得表结构
                DataTable ndt = new DataTable();
                //清空数据
                //ndt.Rows.Clear();
                ndt.TableName = dt.TableName;
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnname = dt.Columns[i].ColumnName;
                    columnname = ConfigurationManager.AppSettings[columnname] != null ? ConfigurationManager.AppSettings[columnname] : columnname;

                    ndt.Columns.Add(columnname, typeof(string));
                    if (cols == "")
                        cols = "[" + columnname + "] Text";
                    else
                        cols += ",[" + columnname + "] Text";
                }
                OleDbCommand com = new OleDbCommand();
                com.Connection = myConn;
                myConn.Open();
                com.CommandText = "CREATE TABLE [Report](" + cols + ")";
                com.ExecuteNonQuery();
                string strCom = "SELECT * FROM [Report$]";
                OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
                //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。   
                //获取insert语句中保留字符（起始位置）  
                builder.QuotePrefix = "[";
                //获取insert语句中保留字符（结束位置）   
                builder.QuoteSuffix = "]";

                //myCommand.Fill(newds, TableName);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //在这里不能使用ImportRow方法将一行导入到news中，
                    //因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。
                    //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState!=Added     
                    DataRow row = ndt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        row[j] = dt.Rows[i][j];
                    }
                    ndt.Rows.Add(row);
                }
                //插入数据
                myCommand.Update(ndt);
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write("<script>alert('create excel failture !" + e.Message + "')</script>");
            }
            finally
            {
                myConn.Close();
            }
            return new FileInfo(file);
        }

        public static bool ExportToExcel(DataTable dt)
        {
            HttpResponse resp = HttpContext.Current.Response;
            try
            {
                resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                resp.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(dt.TableName, System.Text.Encoding.UTF8) + ".xls");
                string colHeaders = "", ls_item = "";
                int i = 0;
                DataRow[] myRow = dt.Select("");
                //取得数据表各列标题,各标题之间以"t分割,最后一个列标题后加回车符 
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                    colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                    //向HTTP输出流中写入取得的数据信息 
                    resp.Write(colHeaders);
                }
                //逐行处理数据  
                foreach (DataRow row in myRow)
                {
                    //在当前行中,逐列获得数据,数据之间以"t分割,结束时加回车符"n
                    for (i = 0; i < row.ItemArray.Length; i++)
                    {
                        ls_item += row[i].ToString() + "\t";
                        ls_item += row[i].ToString() + "\n";
                        //当前行数据写入HTTP输出流,并且置空ls_item以便下行数据
                        resp.Write(ls_item);
                        ls_item = "";
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                resp.Write("<script>alert('导出出错：" + e.Message + "')</script>");
                return false;
            }
        }
        /// <summary>
        /// typeid=="1"时导出为EXCEL格式文件；typeid=="2"时导出为XML格式文件 
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="typeid"></param>
        /// <param name="FileName"></param>
        public static void CreateExcel(DataSet ds, string typeid, string FileName)
        {
            HttpResponse resp;
            resp = HttpContext.Current.Response;
            resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            resp.AppendHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");
            string colHeaders = "", ls_item = "";
            int i = 0;
            //定义表对象与行对像,同时用DataSet对其值进行初始化 
            DataTable dt = ds.Tables[0];
            DataRow[] myRow = dt.Select("");
            // typeid=="1"时导出为EXCEL格式文件；typeid=="2"时导出为XML格式文件 
            if (typeid == "1")
            {
                //取得数据表各列标题,各标题之间以"t分割,最后一个列标题后加回车符 
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    colHeaders += dt.Columns[i].Caption.ToString() + "\t";
                    colHeaders += dt.Columns[i].Caption.ToString() + "\n";
                    //向HTTP输出流中写入取得的数据信息 
                    resp.Write(colHeaders);
                }
                //逐行处理数据  
                foreach (DataRow row in myRow)
                {
                    //在当前行中,逐列获得数据,数据之间以"t分割,结束时加回车符"n
                    for (i = 0; i < row.ItemArray.Length; i++)
                    {
                        ls_item += row[i].ToString() + "\t";
                        ls_item += row[i].ToString() + "\n";
                        //当前行数据写入HTTP输出流,并且置空ls_item以便下行数据
                        resp.Write(ls_item);
                        ls_item = "";
                    }
                }
            }
            else
            {
                if (typeid == "2")
                {
                    //从DataSet中直接导出XML数据并且写到HTTP输出流中 
                    resp.Write(ds.GetXml());
                }
            }
            //写缓冲区中的数据到HTTP头文件中
            resp.End();
        }

        public static void ToExcel(System.Web.UI.Control ctl)
        {
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=Excel.xls");
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Default;
            HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword
            ctl.Page.EnableViewState = false;
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            ctl.RenderControl(hw);
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }

        //用法：ToExcel(datagrid1);

        /// <summary>
        /// dv为要输出到Excel的数据,str为标题名称 
        /// </summary>
        /// <param name="dv"></param>
        /// <param name="str"></param>
        //public static void OutputExcel(DataView dv,string str)
        //{
        //   GC.Collect();
        //   Application excel;// = new Application();
        //   int rowIndex=4;
        //   int colIndex=1;
        //   _Workbook xBk;
        //   _Worksheet xSt;
        //   excel= new ApplicationClass();
        //   xBk = excel.Workbooks.Add(true);
        //   xSt = (_Worksheet)xBk.ActiveSheet;
        //   //
        //   //取得标题
        //   //
        //   foreach(DataColumn col in dv.Table.Columns)
        //    {
        //    colIndex++;
        //    excel.Cells[4,colIndex] = col.ColumnName;
        //    xSt.get_Range(excel.Cells[4,colIndex],excel.Cells[4,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置标题格式为居中对齐
        //   }
        //   //
        //   //取得表格中的数据
        //   //
        //   foreach(DataRowView row in dv)
        //    {
        //    rowIndex ++;
        //    colIndex = 1;
        //    foreach(DataColumn col in dv.Table.Columns)
        //     {
        //     colIndex ++;
        //     if(col.DataType == System.Type.GetType("System.DateTime"))
        //      {
        //      excel.Cells[rowIndex,colIndex] = (Convert.ToDateTime(row[col.ColumnName].ToString())).ToString("yyyy-MM-dd");
        //      xSt.get_Range(excel.Cells[rowIndex,colIndex],excel.Cells[rowIndex,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置日期型的字段格式为居中对齐 
        //     }
        //     else
        //      if(col.DataType == System.Type.GetType("System.String"))
        //      {
        //      excel.Cells[rowIndex,colIndex] = "'"+row[col.ColumnName].ToString();
        //      xSt.get_Range(excel.Cells[rowIndex,colIndex],excel.Cells[rowIndex,colIndex]).HorizontalAlignment = XlVAlign.xlVAlignCenter;//设置字符型的字段格式为居中对齐 
        //     }
        //     else 
        //      {
        //      excel.Cells[rowIndex,colIndex] = row[col.ColumnName].ToString();
        //     }
        //    }
        //   }
        //   //
        //   //加载一个合计行 
        //   //
        //   int rowSum = rowIndex + 1;
        //   int colSum = 2;
        //   excel.Cells[rowSum,2] = "合计";
        //   xSt.get_Range(excel.Cells[rowSum,2],excel.Cells[rowSum,2]).HorizontalAlignment = XlHAlign.xlHAlignCenter;
        //   //
        //   //设置选中的部分的颜色 
        //   //
        //   xSt.get_Range(excel.Cells[rowSum,colSum],excel.Cells[rowSum,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[rowSum,colSum],excel.Cells[rowSum,colIndex]).Interior.ColorIndex = 19;//设置为浅黄色,共计有56种 
        //   //
        //   //取得整个报表的标题 
        //   //
        //   excel.Cells[2,2] = str;
        //   //
        //   //设置整个报表的标题格式 
        //   //
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,2]).Font.Bold = true;
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,2]).Font.Size = 22;
        //   //
        //   //设置报表表格为最适应宽度 
        //   //
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Columns.AutoFit();
        //   //
        //   //设置整个报表的标题为跨列居中 
        //   //
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,colIndex]).Select();
        //   xSt.get_Range(excel.Cells[2,2],excel.Cells[2,colIndex]).HorizontalAlignment = XlHAlign.xlHAlignCenterAcrossSelection;
        //   //
        //   //绘制边框 
        //   //
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,colIndex]).Borders.LineStyle = 1;
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[rowSum,2]).Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThick;//设置左边线加粗 
        //   xSt.get_Range(excel.Cells[4,2],excel.Cells[4,colIndex]).Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThick;//设置上边线加粗 
        //   xSt.get_Range(excel.Cells[4,colIndex],excel.Cells[rowSum,colIndex]).Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThick;//设置右边线加粗 
        //   xSt.get_Range(excel.Cells[rowSum,2],excel.Cells[rowSum,colIndex]).Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThick;//设置下边线加粗 
        //   //
        //   //显示效果 
        //   //
        //   excel.Visible=true;
        //   //xSt.Export(Server.MapPath(".")+""""+this.xlfile.Text+".xls",SheetExportActionEnum.ssExportActionNone,Microsoft.Office.Interop.OWC.SheetExportFormat.ssExportHTML);
        //   xBk.SaveCopyAs(HttpContext.Current.Server.MapPath(".")+""+str+".xls");
        //    ds = null;
        //            xBk.Close(false, null,null);
        //            excel.Quit();
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(xBk);
        //            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(xSt);
        //            xBk = null;
        //            excel = null;
        //   xSt = null;
        //            GC.Collect();
        //   string path = HttpContext.Current.Server.MapPath(this.xlfile.Text+".xls");
        //   System.IO.FileInfo file = new System.IO.FileInfo(path);
        //   HttpContext.Current.Response.Clear();
        //   HttpContext.Current.Response.Charset="GB2312";
        //   HttpContext.Current.Response.ContentEncoding=System.Text.Encoding.UTF8;
        //   // 添加头信息,为"文件下载/另存为"对话框指定默认文件名 
        //   HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(file.Name));
        //   // 添加头信息,指定文件大小,让浏览器能够显示下载进度 
        //   HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
        //   // 指定返回的是一个不能被客户端读取的流,必须被下载 
        //   HttpContext.Current.Response.ContentType = "application/ms-excel";
        //   // 把文件流发送到客户端 
        //    HttpContext.Current.Response.WriteFile(file.FullName);
        //   // 停止页面的执行 
        //   HttpContext.Current.Response.End();
        //}
    }
}
