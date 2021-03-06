using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Autoservisas.Models
{
    public class Reservation
    {
        [DisplayName("Data")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Modelis")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Pagaminimo metai")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime CDate { get; set; }

        [DisplayName("Tūris")]
        [Required]
        public double ESize { get; set; }

        [DisplayName("Suma")]
        [Required]
        public double Sum { get; set; }

        [DisplayName("Pabaigos data")]
        [Required]
        public DateTime EDate { get; set; }

        [DisplayName("Valstybinis numeris")]
        [Required]
        public string CarNumber { get; set; }

        [DisplayName("Darbo kaina")]
        [Required]
        public double WPrice { get; set; }

        [DisplayName("Markė")]
        [Required]
        public IEnumerable<string> Types { get; set; }

        [DisplayName("Markė")]
        [Required]
        public string Type { get; set; }

        [DisplayName("Kuro tipas")]
        [Required]
        public string FuelType { get; set; }

        [DisplayName("Kuro tipas")]
        [Required]
        public IEnumerable<string> FuelTypes { get; set; }

        [DisplayName("Reikalingo meistro kategorija")]
        [Required]
        public IEnumerable<string> Category { get; set; }

        [DisplayName("Reikalingo meistro kategorija")]
        [Required]
        public string Categoryy { get; set; }

        [DisplayName("Simptomo aprašymas")]
        [Required]
        public IEnumerable<string> SymptomData { get; set; }

        [DisplayName("Meistras")]
        [Required]
        public string Mechanic { get; set; }
        public int mID { get; set; }

        public List<Mechanic> mechanikai { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }

        public List<String> GetCategories()
        {
            List<String> Categories = new List<String>();
            connection();

            SqlCommand cmd = new SqlCommand("GetMechanicDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                string category = Convert.ToString(dr["kategorija"]);
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }

            return Categories;
        }

        public List<String> GetSymptoms()
        {
            List<String> Symptoms = new List<String>();
            connection();

            SqlCommand cmd = new SqlCommand("GetSymptomDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                string symptom = Convert.ToString(dr["aprasymas"]);
                if (!Symptoms.Contains(symptom))
                {
                    Symptoms.Add(symptom);
                }
            }

            return Symptoms;
        }

        public class Timee
        {
            [DisplayName("Laikas nuo")]
            public int timefrom { get; set; }
            [DisplayName("Laikas iki")]
            public int timeto { get; set; }
            [DisplayName("Data")]
            public DateTime date { get; set; }
            public int timeid { get; set; }

        }
        public List<Timee> GetTime(int id)
        {
            List<Timee> Time = new List<Timee>();
            connection();

            SqlCommand cmd = new SqlCommand("GetWorkHours", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                int mid = Convert.ToInt32(dr["fk_meistrasid"]);
                if (mid == id)
                {
                    Time.Add(new Timee { 
                        timefrom = Convert.ToInt32(dr["laikas_nuo"]),
                        timeto = Convert.ToInt32(dr["laikas_iki"]),
                        date = Convert.ToDateTime(dr["data"]),
                        timeid = Convert.ToInt32(dr["id_darbo_laikas"])
                    });
                }
            }

            return Time;
        }

        public List<String> GetTypes()
        {
            string[] types = { "bmw", "audi", "opel", "toyota", "skoda", "saab", "seat", "alfa romeo", "fiat", "volkswagen", "volvo", "nissan",
                "kia", "mercedes", "porsche", "citroen", "renault", "peugeot", "dacia", "dodge", "cadilac", "chevrolet", "ford", "chrysler",
                "honda", "hyundai", "jaguar", "jeep", "lexus", "mazda", "mini", "mitsubishi", "subaru" };
            List<String> Types = new List<String>(types);

            return Types;
        }
        public List<String> GetFuelTypes()
        {
            string[] types = { "dyzelinas", "benzinas", "dujos/benzinas", "elektra", "dyzelinas/elektra", "benzinas/elektra" };
            List<String> FuelTypes = new List<String>(types);

            return FuelTypes;
        }

        public List<Mechanic> GetMechanicsFromCategory(string category)
        {
            List<Mechanic> mechanics = new List<Mechanic>();
            connection();

            SqlCommand cmd = new SqlCommand("GetMechanicDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                string cat = Convert.ToString(dr["kategorija"]);
                if (cat == category)
                {
                    mechanics.Add(
                    new Mechanic
                    {
                        MechanicID = Convert.ToInt32(dr["id_naudotojas"]),
                        Picture = Convert.ToString(dr["nuotrauka"]),
                        Category = Convert.ToString(dr["kategorija"]),
                    });
                }
            }
            return mechanics;
        }

        public bool AddUser(Registered_User model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@vardas", model.FName);
            cmd.Parameters.AddWithValue("@pavarde", model.LName);
            cmd.Parameters.AddWithValue("@elpastas", model.Email);
            cmd.Parameters.AddWithValue("@slaptazodis", model.Password);
            cmd.Parameters.AddWithValue("@gimimo_metai", model.Birthday);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
    }
}