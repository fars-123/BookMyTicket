using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace BookMyTicket.Models
{
    public class UserDB
    {
        SqlConnection con = new SqlConnection(@"server=MSI\SQLEXPRESS;database=BookMyTicket;Integrated Security=true;TrustServerCertificate=True"); 

        public string UserRegInsertDB(User_Register objcls)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_UserRegister", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Name", objcls.User_Name);
                cmd.Parameters.AddWithValue("@FullName", objcls.FullName);
                cmd.Parameters.AddWithValue("@Email", objcls.Email);
                cmd.Parameters.AddWithValue("@Address", objcls.Address);
                cmd.Parameters.AddWithValue("@Phone", objcls.Phone);
                cmd.Parameters.AddWithValue("@Role", objcls.Role);
                cmd.Parameters.AddWithValue("@Password", objcls.Password);
                cmd.Parameters.AddWithValue("@IsApproved", objcls.IsApproved);
                cmd.Parameters.AddWithValue("@IsActive", objcls.IsActive);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Successfully Inserted");

            }
            catch (Exception ex)
            {
                if(con.State==ConnectionState.Open)
                {
                    con.Close();
                }
                return ex.Message;
            }
        }
        // 1. Saves the secret code for an email. Returns true if the email was found.
        public bool SaveResetToken(string email, string token)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_SetResetToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Token", token);
                con.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
            finally
            {
                con.Close();
            }
        }

        // 2. Finds the email that owns a secret code. Returns null if the code is not valid.
        public string? GetEmailByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_GetEmailByToken", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);
                con.Open();
                object? result = cmd.ExecuteScalar();
                return result?.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        // 3. Saves the new password. Returns true if it worked.
        public bool ResetPassword(string token, string newPassword)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("Sp_ResetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                con.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result > 0;
            }
            finally
            {
                con.Close();
            }
        }

        // 4. Sends an email.
        public void SendEmail(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("cornerstore.ddc@gmail.com", "mtjr jxtg sgcl hnmp");

            var mail = new MailMessage("cornerstore.ddc@gmail.com", to, subject, body);
            mail.IsBodyHtml = true;

            client.Send(mail);
        }
        
        public User_Register UserLogin(User_Register objcls)
        {
            User_Register obj = new User_Register();

            try
            {
                SqlCommand cmd = new SqlCommand("Sp_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User_Name", objcls.User_Name);
                cmd.Parameters.AddWithValue("@Password", objcls.Password);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj.User_Id = Convert.ToInt32(dr["User_Id"]);
                    obj.Role = dr["Role"].ToString();
                    obj.IsApproved = Convert.ToBoolean(dr["IsApproved"]);
                    obj.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }

                dr.Close();
                con.Close();

                return obj;
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
        public List<Show> View_TheatreShows(int movie_id)
        {
            List<Show> list = new List<Show>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_Select_Theatre", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Movie_Id", movie_id);

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var o = new Show
                    {

                        Screen_Id = Convert.ToInt32(dr["Screen_Id"]),
                        FullName = Convert.ToString(dr["FullName"]),
                        StartTime = (TimeSpan)dr["StartTime"],
                        Movie_Name = Convert.ToString(dr["Movie_Name"])

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
        public List<User_Register> Pending_TheatreOwners()
        {
            List<User_Register> list = new List<User_Register>();

            try
            {
                SqlCommand cmd = new SqlCommand(
                    "Sp_Pending_TheatreOwners", con);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    User_Register obj = new User_Register();

                    obj.User_Id = Convert.ToInt32(dr["User_Id"]);
                    obj.User_Name = dr["User_Name"].ToString();
                    obj.FullName = dr["FullName"].ToString();
                    obj.Email = dr["Email"].ToString();
                    obj.Phone = dr["Phone"].ToString();
                    obj.Address = dr["Address"].ToString();
                    obj.IsApproved = Convert.ToBoolean(dr["IsApproved"]);

                    list.Add(obj);
                }

                dr.Close();
                con.Close();

                return list;
            }
            catch
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }

        public bool Approve_TheatreOwner(int userId)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "Sp_Approve_TheatreOwner", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User_Id", userId);

                con.Open();

                int result = Convert.ToInt32(cmd.ExecuteScalar());

                con.Close();

                return result > 0;
            }
            catch
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                throw;
            }
        }
        public int User_Count()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_user_count", con);
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
        public int Approved_TheatreOwner_Count()
        {
            try
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_approved_theatre_owner_count", con);

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
       



    }
}

