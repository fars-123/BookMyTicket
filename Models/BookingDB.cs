using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace BookMyTicket.Models
{
    public class BookingDB
    {
        SqlConnection con = new SqlConnection(
            @"server=MSI\SQLEXPRESS;database=BookMyTicket;Integrated Security=true;TrustServerCertificate=True");

        public void Insert_Booking(Booking obj)
        {
            SqlCommand cmd = new SqlCommand("sp_insert_booking", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@User_Id", obj.User_Id);
            cmd.Parameters.AddWithValue("@Screen_Id", obj.Screen_Id);
            cmd.Parameters.AddWithValue("@ShowDate", obj.ShowDate.Date);
            cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = obj.StartTime;
            cmd.Parameters.AddWithValue("@TicketPrice", obj.TicketPrice);
            cmd.Parameters.AddWithValue("@SeatCount", obj.SeatCount);
            cmd.Parameters.AddWithValue("@TotalAmount", obj.TotalAmount);

            string seats = string.Join(",", obj.Seats ?? new List<string>());

            cmd.Parameters.AddWithValue("@Seats", seats);
            cmd.Parameters.AddWithValue("@PaymentStatus", "Pending");

            if (string.IsNullOrEmpty(obj.RazorpayOrderId))
            {
                cmd.Parameters.AddWithValue("@RazorpayOrderId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RazorpayOrderId", obj.RazorpayOrderId);
            }

            if (string.IsNullOrEmpty(obj.RazorpayPaymentId))
            {
                cmd.Parameters.AddWithValue("@RazorpayPaymentId", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@RazorpayPaymentId", obj.RazorpayPaymentId);
            }

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public void Update_Payment(string orderId, string paymentId)
        {
            SqlCommand cmd = new SqlCommand(
                "sp_update_booking_payment", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@OrderId", orderId);
            cmd.Parameters.AddWithValue("@PaymentId", paymentId);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        public List<string> Get_Unavailable_Seats(
            int Screen_Id,
            DateTime ShowDate,
            TimeSpan StartTime)
        {
            List<string> seats = new List<string>();

            SqlCommand cmd = new SqlCommand(
                "sp_Get_Unavailable_Seats", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Screen_Id", Screen_Id);
            cmd.Parameters.AddWithValue("@ShowDate", ShowDate.Date);
            cmd.Parameters.Add("@StartTime", SqlDbType.Time).Value = StartTime;

            try
            {
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    seats.Add(dr["SeatName"].ToString());
                }

                dr.Close();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return seats;
        }
        public int Booking_Count()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_booking_count", con);
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
        public int Theatre_Booking_Count(int User_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_theatre_booking_count", con);
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
        public decimal Theatre_Total_Revenue(int User_Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_theatre_total_revenue", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@User_Id", User_Id);

                con.Open();

                decimal revenue = Convert.ToDecimal(cmd.ExecuteScalar());

                con.Close();

                return revenue;
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
        public List<Booking> Theatre_View_Bookings(int User_Id)
        {
            List<Booking> list = new List<Booking>();

            SqlCommand cmd = new SqlCommand("sp_theatre_view_bookings", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@User_Id", User_Id);

            try
            {
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Booking obj = new Booking();

                    obj.Booking_Id = Convert.ToInt32(dr["Booking_Id"]);
                    obj.FullName = dr["FullName"].ToString();
                    obj.Movie_Name = dr["Movie_Name"].ToString();
                    obj.ShowDate = Convert.ToDateTime(dr["ShowDate"]);
                    obj.StartTime = (TimeSpan)dr["StartTime"];
                    obj.Seats = dr["Seats"].ToString().Split(',').ToList();
                    obj.SeatCount = Convert.ToInt32(dr["SeatCount"]);
                    obj.TotalAmount = Convert.ToDecimal(dr["TotalAmount"]);
                    obj.PaymentStatus = dr["PaymentStatus"].ToString();

                    list.Add(obj);
                }

                dr.Close();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return list;
        }

    }
}
