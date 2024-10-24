using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static SelfFunded.Models.BenefitCategory;

namespace SelfFunded.DAL
{
    public class BenefitCategoryDal
    {
        private readonly CommonDal commondal;
        private readonly string conString;

        public BenefitCategoryDal(IConfiguration configuration)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public List<BindBenefitCategory> getBenefitCategory()
        {
            List<BindBenefitCategory> benfitcat = new List<BindBenefitCategory>();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(conString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetBenefitCategoryMaster", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    benfitcat.Add(new BindBenefitCategory
                    {
                        benefitCatId = Convert.ToInt32(dr["benefitCatId"]),
                        benefitCategory = dr["benefitCategory"].ToString(),
                    });
                }
                return benfitcat;
            }
            catch (Exception ex)
            {
                commondal.LogError("getBenefitCategory", "BenefitCategoryController", ex.Message, "BenefitCategoryDal");
                return benfitcat; // Return empty list on error
            }
            finally
            {
                // Ensure connection is closed even if an exception occurs
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    }
}
