using Microsoft.Data.SqlClient;
using System.Data;
namespace BookMyTicket.Models
{
    public class ScreenDB
    {
        SqlConnection con = new SqlConnection(@"server=MSI\SQLEXPRESS;database=BookMyTicket;Integrated Security=true;TrustServerCertificate=True");

        public string Screen_Insert(Screen objcls)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Insert_Screen", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", objcls.User_Id);
                cmd.Parameters.AddWithValue("@Movie_Id", objcls.Movie_Id);
                cmd.Parameters.AddWithValue("@Movie_Name", objcls.Movie_Name);
                cmd.Parameters.AddWithValue("@ScreenName", objcls.ScreenName);
                cmd.Parameters.AddWithValue("@Rows", objcls.Rows);
                cmd.Parameters.AddWithValue("@Columns", objcls.Columns);

                cmd.Parameters.AddWithValue("@ShowDate", objcls.ShowDate);
                cmd.Parameters.AddWithValue("@StartTime", objcls.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", objcls.EndTime);
                cmd.Parameters.AddWithValue("@TicketPrice", objcls.TicketPrice);



                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Screen Added");

            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                throw;
            }
        }
        public int Screen_Count(int User_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_screen_count", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User_Id", User_Id);

                con.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return count;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public int Show_Count(int User_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_show_count", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User_Id", User_Id);

                con.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return count;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }

        public List<Screen> Screen_View(int userid)
        {
            var list=new List<Screen>();
            try
            {
                
                SqlCommand cmd = new SqlCommand("Sp_View_Screen", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Id", userid);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    var o = new Screen
                    {
                        Screen_Id = Convert.ToInt32(dr["Screen_Id"]),
                        User_Id = Convert.ToInt32(dr["User_Id"]),
                        Movie_Id = Convert.ToInt32(dr["Movie_Id"]),
                        Movie_Name = dr["Movie_Name"].ToString(),
                        ScreenName = dr["ScreenName"].ToString(),
                        Rows = Convert.ToInt32(dr["Rows"]),
                        Columns = Convert.ToInt32(dr["Columns"]),
                        ShowDate = Convert.ToDateTime(dr["ShowDate"]),
                        StartTime = TimeSpan.Parse(dr["StartTime"].ToString()),
                        EndTime = TimeSpan.Parse(dr["EndTime"].ToString()),
                        TicketPrice = Convert.ToDecimal(dr["TicketPrice"]),
                        IsEnable = Convert.ToBoolean(dr["IsEnable"])

                    };
                    list.Add(o);
                }

                con.Close();
                return list;

            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                throw;
            }
        }
        public List<Movie> Movie_Select()
        {
            var list = new List<Movie>();
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_select_Movie", con);
                cmd.CommandType = CommandType.StoredProcedure;
              

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var d = new Movie
                    {
                        Movie_Id = Convert.ToInt32(dr["Movie_Id"]),
                        Movie_Name = dr["Movie_Name"].ToString(),
                     

                    };
                    list.Add(d);
                }

                con.Close();
                return list;

            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                throw;
            }
        }
        public List<Screen> Get_Screen(int screenid)
        {
            List<Screen> list = new List<Screen>();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_Get_Screen_Id", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Screen_Id", screenid);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var o = new Screen
                    {
                        Screen_Id = Convert.ToInt32(dr["Screen_Id"]),
                        ScreenName = dr["ScreenName"].ToString(),
                        Rows = Convert.ToInt32(dr["Rows"]),
                        Columns = Convert.ToInt32(dr["Columns"]),

                        ShowDate = Convert.ToDateTime(dr["ShowDate"]),
                        StartTime = TimeSpan.Parse(dr["StartTime"].ToString())
                    };

                    list.Add(o);
                }

                dr.Close();
                con.Close();

                return list;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public decimal Get_TicketPrice(int Screen_Id)
        {

            SqlCommand cmd = new SqlCommand("Sp_Get_TicketPrice", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Screen_Id", Screen_Id);

            con.Open();

            decimal price = Convert.ToDecimal(cmd.ExecuteScalar());

            con.Close();

            return price;
        }
        public List<Screen> Edit_Screen(int screenid)
        {
            List<Screen> list = new List<Screen>();

            try
            {
                SqlCommand cmd = new SqlCommand("sp_Edit_Screen", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Screen_Id", screenid);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Screen obj = new Screen();

                    obj.Screen_Id = Convert.ToInt32(dr["Screen_Id"]);
                    obj.User_Id = Convert.ToInt32(dr["User_Id"]);
                    obj.Movie_Id = Convert.ToInt32(dr["Movie_Id"]);
                    obj.Movie_Name = dr["Movie_Name"].ToString();
                    obj.ScreenName = dr["ScreenName"].ToString();
                    obj.Rows = Convert.ToInt32(dr["Rows"]);
                    obj.Columns = Convert.ToInt32(dr["Columns"]);
                    obj.ShowDate = Convert.ToDateTime(dr["ShowDate"]);
                    obj.StartTime = TimeSpan.Parse(dr["StartTime"].ToString());
                    obj.EndTime = TimeSpan.Parse(dr["EndTime"].ToString());
                    obj.TicketPrice = Convert.ToDecimal(dr["TicketPrice"]);

                    list.Add(obj);
                }

                dr.Close();
                con.Close();

                return list;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public string Update_Screen(Screen obj)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Update_Screen", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Screen_Id", obj.Screen_Id);
                cmd.Parameters.AddWithValue("@Movie_Id", obj.Movie_Id);
                cmd.Parameters.AddWithValue("@Movie_Name", obj.Movie_Name);
                cmd.Parameters.AddWithValue("@ScreenName", obj.ScreenName);
                cmd.Parameters.AddWithValue("@Rows", obj.Rows);
                cmd.Parameters.AddWithValue("@Columns", obj.Columns);
                cmd.Parameters.AddWithValue("@ShowDate", obj.ShowDate);
                cmd.Parameters.AddWithValue("@StartTime", obj.StartTime);
                cmd.Parameters.AddWithValue("@EndTime", obj.EndTime);
                cmd.Parameters.AddWithValue("@TicketPrice", obj.TicketPrice);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Show Updated Successfully";
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public int Check_Show_Booking(int Screen_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Check_Show_Booking", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Screen_Id", Screen_Id);

                con.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return count;
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public string Delete_Screen(int Screen_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Delete_Screen", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Screen_Id", Screen_Id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return "Show Deleted Successfully";
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public void Show_IsEnable_Update(int Screen_Id, int User_Id, bool IsEnable)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Update_Show_IsEnable", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Screen_Id", Screen_Id);
                cmd.Parameters.AddWithValue("@User_Id", User_Id);
                cmd.Parameters.AddWithValue("@IsEnable", IsEnable);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }

    }
}
