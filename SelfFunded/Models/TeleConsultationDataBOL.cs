namespace SelfFunded.Models
{
    public class TeleConsultationDataBOL
    {
        public int teleConsultationID { get; set; }
        public int userCode { get; set; }
        public string patientName { get; set; }
        public string doctorSignature { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public long contactNo { get; set; }
        public string preferredDate { get; set; }
        public string preferredTime { get; set; }
        public int responseCode { get; set; }
        public string chiefComplaint { get; set; }
        public string diabetesMellitus { get; set; }
        public string hypertension { get; set; }
        public string heartDisease { get; set; }
        public string cancer { get; set; }
        public string stroke { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public string bp { get; set; }
        public int pulse { get; set; }
        public int bodyTemperature { get; set; }
        public int bodyTemperatureTypeID { get; set; }
        public string historyOfPresentIllness { get; set; }
        public string treatmentAdvice { get; set; }
        public string investigationAdvised { get; set; }
        public string additionalNotes { get; set; }
        public string refferTo { get; set; }
        public int followUpDays { get; set; }
        public int provisionalDiagnosis { get; set; }
        public string provisionalDiagnosisComment { get; set; }
        public string doctorName { get; set; }
        public string specialityName { get; set; }
        public DateTime? appointmentCreatedDatetime { get; set; }
        public DateTime? appointmentCreatedDate { get; set; }
        public string fileName { get; set; }
        public int doctorID { get; set; }

        // List of medicines
        public List<MedicineAdvisedBOL> medicineAdvisedList { get; set; }
    }
}
