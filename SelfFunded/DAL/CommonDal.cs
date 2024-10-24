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
    public class CommonDal
    {

        public string conString;

        // Parameterized constructor
        public CommonDal(IConfiguration configuration)
        {
            Initialize(configuration);
            Console.WriteLine("inside parameterized: " + conString);
        }

        private void Initialize(IConfiguration configuration)
        {
            conString = configuration?.GetConnectionString("adoConnectionstring");
        }

        // Default constructor
        public CommonDal() : this(null)
        {
            Console.WriteLine("inside default: " + conString);
        }

        public void LogError(string methodName, string controllerName, string errorMessage, string dalName)
        {
            ErrorLog errorLog = new ErrorLog
            {
                error = errorMessage,
                methodName = methodName,
                controllerName = controllerName,
                dalName = string.IsNullOrEmpty(dalName) ? null : dalName
            };
            InsertErrorLog(errorLog);
        }

        public List<CommonCountry> getCountry()
        {
            List<CommonCountry> countries = new List<CommonCountry>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetCountry]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    countries.Add(new CommonCountry
                    {
                        countryName = dr["countryName"].ToString(),
                        countryID = Convert.ToInt32(dr["countryId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetCountry", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return countries;
        }

        public List<CommonState> getState(int countryID)
        {
            List<CommonState> states = new List<CommonState>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetState]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@countryID", countryID);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    states.Add(new CommonState
                    {
                        stateName = dr["stateName"].ToString(),
                        stateID = Convert.ToInt32(dr["stateID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetState", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return states;
        }

        public List<CommonCity> getCity(int stateID)
        {
            List<CommonCity> cities = new List<CommonCity>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetCity]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@stateID", stateID);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    cities.Add(new CommonCity
                    {
                        cityName = dr["cityName"].ToString(),
                        cityID = Convert.ToInt32(dr["cityId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetCity", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return cities;
        }

        public List<BindInsurance> getInsuranceName()
        {
            List<BindInsurance> insuranceNames = new List<BindInsurance>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetInsuranceName]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    insuranceNames.Add(new BindInsurance
                    {
                        insuranceCompany = dr["InsuranceCompany"].ToString(),
                        insuranceCompanyId = Convert.ToInt32(dr["InsuranceCompanyId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetInsuranceName", "CommonControoler", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return insuranceNames;
        }

        private void InsertErrorLog(ErrorLog log)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertErrorLog", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Error", log.error);
                        cmd.Parameters.AddWithValue("@MethodName", log.methodName);
                        cmd.Parameters.AddWithValue("@ControllerName", log.controllerName);
                        cmd.Parameters.AddWithValue("@DalName", log.dalName ?? (object)DBNull.Value);

                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("InsertErrorLog", "CommonController", ex.Message, "CommonDal");
                Console.WriteLine("Error logging failed: " + ex.Message);
            }
        }
        public List<CorporateMaster> getCorporate()
        {
            List<CorporateMaster> corporates = new List<CorporateMaster>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetAllCorporateMaster]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    corporates.Add(new CorporateMaster
                    {
                        corporateName = dr["corporateName"].ToString(),
                        corporateId = Convert.ToInt32(dr["corporateId"].ToString())
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetCorporate", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return corporates;
        }




        public string insertErrorLog(ErrorLog log)
        {
            SqlConnection connection = null;

            try
            {
                int id = 0;
                using (connection = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_InsertErrorLog", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Error", log.error);
                        cmd.Parameters.AddWithValue("@MethodName", log.methodName);
                        cmd.Parameters.AddWithValue("@ControllerName", log.controllerName);
                        cmd.Parameters.AddWithValue("@DalName", log.dalName != null ? log.dalName : (object)DBNull.Value); // Pass DBNull.Value for null values

                        connection.Open();
                        id = cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                if (id > 0)
                {
                    return "Data saved successfully";
                }
                else
                {
                    return "Error occurred while saving data";
                }
            }
            catch (Exception ex)
            {
                LogError("insertErrorLog", "CommonController", ex.Message, "CommonDal");
                return "Error occurred: " + ex.Message;
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public List<BindIndustry> getIndustry()
        {
            List<BindIndustry> industries = new List<BindIndustry>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetIndustry]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    industries.Add(new BindIndustry
                    {
                        industryName = dr["IndustryName"].ToString(),
                        industryId = Convert.ToInt32(dr["IndustryId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetIndustry", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return industries;
        }

        public List<BindLevel1> getLevel1(int id)
        {
            List<BindLevel1> level1Items = new List<BindLevel1>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetLevel1]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@BenefitCatId", id);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    level1Items.Add(new BindLevel1
                    {
                        level1Master = dr["level1Master"].ToString(),
                        level1Id = dr["level1Id"].ToString(),
                        level1TransId = dr["Level1TransId"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetLevel1", "CommonController", ex.Message, "CommonDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return level1Items;
        }

        public List<BindLevel2> getLevel2(int benefitId, int level1id)
        {
            List<BindLevel2> level2Items = new List<BindLevel2>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetLevel2]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@BenefitCatId", benefitId);
            da.SelectCommand.Parameters.AddWithValue("@Level1Id", level1id);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    level2Items.Add(new BindLevel2
                    {
                        level2Master = dr["Level2Master"].ToString(),
                        level2Id = dr["Level2Id"].ToString(),
                        level2TransId = dr["Level2TransId"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetLevel2", "CommonController", ex.Message, "CommonDal.getLevel2");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return level2Items;
        }


        public List<BindLevel3> getLevel3(int benefitId, int level1id, int level2id)
        {
            List<BindLevel3> level3Items = new List<BindLevel3>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetLevel3]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@BenefitCatId", benefitId);
            da.SelectCommand.Parameters.AddWithValue("@Level1Id", level1id);
            da.SelectCommand.Parameters.AddWithValue("@Level2Id", level2id);

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    level3Items.Add(new BindLevel3
                    {
                        level3Master = dr["Level3Master"].ToString(),
                        level3Code = dr["Level3Code"].ToString(),
                        level3TransId = dr["Level3TransId"].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetLevel3", "CommonController", ex.Message, "CommonDal.getLevel3");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return level3Items;
        }


        public List<BindStatus> getstatus()
        {
            List<BindStatus> statuses = new List<BindStatus>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[GetStatus]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    statuses.Add(new BindStatus
                    {
                        status = dr["Name"].ToString(),
                        statusId = Convert.ToInt32(dr["StatusID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetStatus", "CommonController", ex.Message, "CommonDal.getstatus");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return statuses;
        }


        public List<BindRelation> getRealtion()
        {
            List<BindRelation> relations = new List<BindRelation>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetRelation]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    relations.Add(new BindRelation
                    {
                        relation = dr["Relation"].ToString(),
                        relationId = Convert.ToInt32(dr["RelationId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetRelation", "CommonController", ex.Message, "CommonDal.getRealtion");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return relations;
        }


        public List<BindEmployeetype> getEmployeeType()
        {
            List<BindEmployeetype> employeeTypes = new List<BindEmployeetype>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetEmployeetype]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    employeeTypes.Add(new BindEmployeetype
                    {
                        employeetype = dr["Name"].ToString(),
                        employeetypeid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetEmployeeType", "CommonController", ex.Message, "CommonDal.getEmployeeType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return employeeTypes;
        }


        public List<BindIntimationType> getIntimationType()
        {
            List<BindIntimationType> intimationTypes = new List<BindIntimationType>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetIntimationtype]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    intimationTypes.Add(new BindIntimationType
                    {
                        intimationtype = dr["Name"].ToString(),
                        intimationtypeid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetIntimationType", "CommonController", ex.Message, "CommonDal.getIntimationType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return intimationTypes;
        }


        public List<BindIntimationFrom> getIntimationFrom()
        {
            List<BindIntimationFrom> intimationFroms = new List<BindIntimationFrom>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetIntimationFrom]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    intimationFroms.Add(new BindIntimationFrom
                    {
                        intimationfrom = dr["Name"].ToString(),
                        intimationfromid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetIntimationFrom", "CommonController", ex.Message, "CommonDal.getIntimationFrom");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return intimationFroms;
        }


        public List<BindClaimType> getClaimType()
        {
            List<BindClaimType> claimTypes = new List<BindClaimType>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetClaimType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    claimTypes.Add(new BindClaimType
                    {
                        claimtype = dr["Name"].ToString(),
                        claimid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetClaimType", "CommonController", ex.Message, "CommonDal.getClaimType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return claimTypes;
        }


        public List<BindCaseType> getCaseType()
        {
            List<BindCaseType> caseTypes = new List<BindCaseType>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetCaseType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    caseTypes.Add(new BindCaseType
                    {
                        casetype = dr["Name"].ToString(),
                        caseid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetCaseType", "CommonController", ex.Message, "CommonDal.getCaseType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return caseTypes;
        }


        public List<BindRequestType> getRequestType()
        {
            List<BindRequestType> requestTypes = new List<BindRequestType>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetRequestType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    requestTypes.Add(new BindRequestType
                    {
                        requesttype = dr["Name"].ToString(),
                        reqtypeid = Convert.ToInt32(dr["GMTID"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetRequestType", "CommonController", ex.Message, "CommonDal.getRequestType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return requestTypes;
        }
        public List<IntimationSheetInbound> getIntimationDetailsForClaim()
        {
            List<IntimationSheetInbound> clmdtls = new List<IntimationSheetInbound>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[Usp_GetIntimationDetailsForClaim]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    clmdtls.Add(new IntimationSheetInbound
                    {

                        srNo = Convert.ToInt32(dr["SrNo"]),
                        intimationId = Convert.ToInt32(dr["intimationId"]),
                        insuranceCompanyId = Convert.ToInt32(dr["insuranceCompanyId"]),
                        intimationNo = dr["IntimationNo"].ToString(),
                        insuredName = dr["InsuredName"].ToString(),
                        primaryMember = dr["PrimaryMember"].ToString(),
                        dateOfIntimation = dr["DateOfIntimation"] != DBNull.Value ? Convert.ToDateTime(dr["DateOfIntimation"]) : (DateTime?)null,
                        timeOfIntimation = dr["TimeOfIntimation"].ToString(),
                        policyNo = dr["PolicyNo"].ToString(),
                        caseType = dr["CaseType"].ToString(),
                        claimTypeId = Convert.ToInt32(dr["ClaimTypeId"].ToString()),
                        claimType = dr["ClaimType"].ToString(),

                    });
                }
            }
            catch (Exception ex)
            {
                LogError("getIntimationDetailsForClaim", "IntimationSheetInBoundControllerController", ex.Message, "IntimationSheetInboundDal");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return clmdtls;
        }
        public List<BindLoginType> getGetLoginType()
        {
            List<BindLoginType> loginTypes = new List<BindLoginType>();
            SqlConnection connection = new SqlConnection(conString);
            SqlDataAdapter da = new SqlDataAdapter("[SP_GetLoginType]", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            try
            {
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    loginTypes.Add(new BindLoginType
                    {
                        loginType = dr["Logintype"].ToString(),
                        loginTypeId = Convert.ToInt32(dr["LoginTypeId"]),
                    });
                }
            }
            catch (Exception ex)
            {
                LogError("GetLoginType", "CommonController", ex.Message, "CommonDal.getGetLoginType");
                throw; // Re-throwing to propagate the exception to the caller
            }
            finally
            {
                connection.Close();
            }

            return loginTypes;
        }
        private static readonly string[] Ones = { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        private static readonly string[] Teens = { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] Tens = { "", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
        private static readonly string[] Thousands = { "", "Thousand", "Lakh", "Crore" };

        //public string ConvertToWords(int number)
        //{
        //    if (number == 0) return "Zero Only";

        //    if (number < 0) return "Negative " + ConvertToWords(Math.Abs(number));

        //    string words = "";

        //    int croreCounter = 0;
        //    while (number > 0)
        //    {
        //        if (number % 10000000 != 0) // 1 crore = 10 million
        //        {
        //            words = ConvertLakhs((int)(number % 10000000)) + Thousands[croreCounter] + " " + words;
        //        }
        //        number /= 10000000;
        //        croreCounter++;
        //    }

        //    return words.Trim() + "Only";
        //}

        private static string ConvertLakhs(int number)
        {
            string words = "";

            if (number >= 100000)
            {
                words += ConvertHundreds(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if (number > 0)
            {
                words += ConvertHundreds(number) + " ";
            }

            return words.Trim();
        }

        private static string ConvertHundreds(int number)
        {
            string words = "";

            if (number >= 100)
            {
                words += Ones[number / 100] + " Hundred ";
                number %= 100;
            }

            if (number >= 10 && number < 20)
            {
                words += Teens[number - 11] + " ";
            }
            else
            {
                words += Tens[number / 10] + " ";
                number %= 10;
            }

            words += Ones[number] + " ";

            return words.Trim();
        }
    }
}