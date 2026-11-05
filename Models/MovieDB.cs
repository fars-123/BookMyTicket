using System.Data;
using Microsoft.Data.SqlClient;

namespace BookMyTicket.Models
{
    public class MovieDB
    {
        SqlConnection con = new SqlConnection(@"server=MSI\SQLEXPRESS;database=BookMyTicket;Integrated Security=true;TrustServerCertificate=True");

        public string Movie_Insert(Movie objcls)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Insert_Movie", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Movie_Name", objcls.Movie_Name);
                cmd.Parameters.AddWithValue("@Movie_Description", objcls.Movie_Description);
                cmd.Parameters.AddWithValue("@Language", objcls.Language);
                cmd.Parameters.AddWithValue("@Movie_Image", objcls.Movie_Image);
                cmd.Parameters.AddWithValue("@Date", objcls.Date);
                cmd.Parameters.AddWithValue("@Genre", objcls.Genre);
                cmd.Parameters.AddWithValue("@Year", objcls.Year);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Movie Successfully Inserted");

            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return ex.Message;
            }
        }
        public List<Movie> Movie_View()
        {
            var list = new List<Movie>();
            try
            {

                SqlCommand cmd = new SqlCommand("Sp_select_Movie_Img", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var o = new Movie
                    {
                        Movie_Id = Convert.ToInt32(dr["Movie_Id"]),
                        Movie_Image = dr["Movie_Image"].ToString(),
                        Movie_Name = dr["Movie_Name"].ToString(),
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
        public int Movie_Count()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Count_Movies", con);
                cmd.CommandType = CommandType.StoredProcedure;

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
        public void Movie_IsEnable_Update(int Movie_Id, bool IsEnable)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_Update_Movie_IsEnable", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Movie_Id", Movie_Id);
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
