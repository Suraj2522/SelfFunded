using SelfFunded.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Security.AccessControl;

namespace SelfFunded.DAL
{
    public class DashboardDal
    {
        CommonDal commondal;
        private readonly string conString;

        public DashboardDal(IConfiguration configuration, CommonDal commondal)
        {
            commondal = new CommonDal(configuration);
            conString = configuration["ConnectionStrings:adoConnectionstring"] ?? "";
        }

        public List<Dashboard> getClaimCountByStatus()
        {
            List<Dashboard> status = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[Usp_GetClaimCountByStatus]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    status.Add(new Dashboard
                    {
                        //claimStatus = (dr["claimStatus"].ToString),
                        claimCount = dr["claimCount"].ToString(),
                        status = dr["status"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("getClaimCountByStatus", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return status;
        }

        public List<Dashboard> getStatusCountByClaimType()
        {
            List<Dashboard> clmtype = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[GetStatusCountByClaimType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    clmtype.Add(new Dashboard
                    {
                        claimStatus = dr["claimStatus"].ToString(),
                        claimCount = dr["claimCount"].ToString(),
                        claimType = dr["claimType"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetStatusCountByClaimType", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return clmtype;
        }
        public List<Dashboard> getClaimCountByRecStatus()
        {
            List<Dashboard> recstatus = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getClaimCountByRecStatus]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    recstatus.Add(new Dashboard
                    {
                        isactive = dr["isactive"].ToString(),
                        claimCount = dr["claimCount"].ToString(),
                        claimType = dr["claimType"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetStatusCountByClaimType", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return recstatus;
        }
        public List<Dashboard> getPreAuthCountByStatus()
        {
            List<Dashboard> status = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getPreAuthCountByStatus]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.CommandTimeout = 600;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    status.Add(new Dashboard
                    {
                        //claimStatus = Convert.ToInt32(dr["claimStatus"]),
                        preAuthCount = dr["preAuthCount"].ToString(),
                        status = dr["status"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountByStatus", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return status;
        }
        public List<Dashboard> getPreAuthCountCaseType()
        {
            List<Dashboard> casetype = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getPreAuthCountCaseType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    casetype.Add(new Dashboard
                    {
                        preAuthCount = dr["preAuthCount"].ToString(),
                        caseType = dr["caseType"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountCaseType", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return casetype;
        }

        public List<Dashboard> getPreAuthCountRecStatus()
        {
            List<Dashboard> recstatus = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getPreAuthCountRecStatus]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    recstatus.Add(new Dashboard
                    {
                        preAuthCount = dr["preAuthCount"].ToString(),
                        recStatus = dr["recStatus"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetPreAuthCountRecStatus", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return recstatus;
        }
        public List<Dashboard> getEnrollmentAgeData()
        {
            List<Dashboard> age = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getEnrollmentAgeData]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    age.Add(new Dashboard
                    {
                        ageGroup = dr["ageGroup"].ToString(),
                        male = Convert.ToInt32(dr["male"]),
                        female = Convert.ToInt32(dr["female"]),
                        total = Convert.ToInt32(dr["total"]),
                        percentage = dr["percentage"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetEnrollmentAgeData", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return age;
        }
        public List<Dashboard> getCountEnrollmentType()
        {
            List<Dashboard> type = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getCountEnrollmentType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    type.Add(new Dashboard
                    {
                        typeOfEnrollment = dr["typeOfEnrollment"].ToString(),
                        enrollmentCount = dr["enrollmentCount"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetCountEnrollmentType", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return type;
        }
        public List<Dashboard> getEnrollmentRelationWiseData()
        {
            List<Dashboard> relation = new List<Dashboard>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[getEnrollmentRelationWiseData]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    relation.Add(new Dashboard
                    {
                        relationType = dr["relationType"].ToString(),
                        male = Convert.ToInt32(dr["male"]),
                        female = Convert.ToInt32(dr["female"]),
                        total = Convert.ToInt32(dr["total"]),
                        other = Convert.ToInt32(dr["other"])
                    });
                }
            }
            catch (Exception ex)
            {
                commondal.LogError("GetEnrollmentRelationWiseData", "DashboardController", ex.Message, "DashboardnDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return relation;
        }
        public List<Dashboard> getIntimationCountByYear()

        { 
            List<Dashboard> year = new List<Dashboard>();

            SqlConnection connection = new SqlConnection(conString);

            SqlDataAdapter da = new SqlDataAdapter("[GetIntimationCountByYear]", connection);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try

            {

                DataTable dt = new DataTable();

                connection.Open();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)

                {

                    year.Add(new Dashboard

                    {

                        intimationCount = dr["intimationCount"].ToString(),

                        intimationYear = dr["intimationYear"].ToString()

                    });

                }

            }

            catch (Exception ex)

            {

                commondal.LogError("GetIntimationCountByYear", "DashboardController", ex.Message, "DashboardnDal");

                throw; // Re-throwing to propagate the exception to the caller

            }

            finally

            {

                connection.Close();

            }

            return year;

        }

        public List<Dashboard> getClaimCountByClaimType()

        {

            List<Dashboard> clmtype = new List<Dashboard>();

            SqlConnection connection = new SqlConnection(conString);

            SqlDataAdapter da = new SqlDataAdapter("[GetClaimCountByClaimType]", connection);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try

            {

                DataTable dt = new DataTable();

                connection.Open();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)

                {

                    clmtype.Add(new Dashboard

                    {

                        claimType = dr["claimType"].ToString(),

                        claimCount = dr["claimCount"].ToString()

                    });

                }

            }

            catch (Exception ex)

            {

                commondal.LogError("getClaimCountByClaimType", "DashboardController", ex.Message, "DashboardnDal");

                throw; // Re-throwing to propagate the exception to the caller

            }

            finally

            {

                connection.Close();

            }

            return clmtype;

        }

        public List<Dashboard> getIntimationCountByInsuranceCompany()

        {

            List<Dashboard> insurance = new List<Dashboard>();

            SqlConnection connection = new SqlConnection(conString);

            SqlDataAdapter da = new SqlDataAdapter("[GetIntimationCountByInsuranceCompany]", connection);

            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try

            {

                DataTable dt = new DataTable();

                connection.Open();

                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)

                {

                    insurance.Add(new Dashboard

                    {

                        insuranceCompany = dr["insuranceCompany"].ToString(),

                        intimationCount = dr["intimationCount"].ToString()

                    });

                }

            }

            catch (Exception ex)

            {

                commondal.LogError("getIntimationCountByInsuranceCompany", "DashboardController", ex.Message, "DashboardnDal");

                throw; // Re-throwing to propagate the exception to the caller

            }

            finally

            {

                connection.Close();

            }

            return insurance;

        }

    }

}
