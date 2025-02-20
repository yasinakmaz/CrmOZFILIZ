namespace Crm.Model
{
    public record TblWaiterList
    {
        [Key]
        public Guid IND { get; set; }
        public Guid AddRecordPersonInd { get; set; }
        public Guid BusinessInd { get; set; }
        public string BusinessAuthNameAndSurname { get; set; }
        public string BusinessPhoneNumber { get; set; }
        public string BusinessReservePhoneNumber { get; set; }
        public string BusinessAnydeskNumber { get; set; }
        public string BusinessProblem { get; set; }
        public Guid BusinessProblemProgramInd { get; set; }
        public DateTime AddRecordDate { get; set; }
        public DateTime RecordLastUpdateDate { get; set; }
        public bool Meeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public Guid BusinessAgremeentInd { get; set; }
        public Guid MomentPersonInd { get; set; }
        public Guid RecordStatus { get; set; }
    }
}
