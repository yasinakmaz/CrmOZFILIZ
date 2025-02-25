namespace Crm.RandomData
{
    public class DataSeeder
    {
        public static void SeedData(int count)
        {
            using (var context = new AppDbContext(SqlServices.SqlConnectionString))
            {
                var faker = new Faker<TblWaiterList>()
                    .RuleFor(t => t.IND, f => Guid.NewGuid())
                    .RuleFor(t => t.AddRecordPersonInd, f => Guid.NewGuid())
                    .RuleFor(t => t.BusinessSelection, f => f.Random.Bool())
                    .RuleFor(t => t.BusinessInd, f => Guid.NewGuid())
                    .RuleFor(t => t.BusinessAuthNameAndSurname, f => f.Name.FullName())
                    .RuleFor(t => t.BusinessPhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(t => t.BusinessReservePhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(t => t.BusinessAnydeskNumber, f => f.Random.Replace("###-###-###"))
                    .RuleFor(t => t.BusinessProblem, f => f.Lorem.Sentence())
                    .RuleFor(t => t.BusinessProblemProgramInd, f => Guid.NewGuid())
                    .RuleFor(t => t.AddRecordDate, f => f.Date.Past(1))
                    .RuleFor(t => t.RecordLastUpdateDate, f => f.Date.Recent())
                    .RuleFor(t => t.Meeting, f => f.Random.Bool())
                    .RuleFor(t => t.MeetingDate, f => f.Date.Future())
                    .RuleFor(t => t.BusinessAgremeentInd, f => Guid.NewGuid())
                    .RuleFor(t => t.MomentPersonInd, f => Guid.NewGuid())
                    .RuleFor(t => t.RecordStatusInd, f => Guid.NewGuid());

                var records = faker.Generate(count);

                context.TBLRECORDLIST.AddRange(records);
                context.SaveChanges();
            }
        }
    }

}
