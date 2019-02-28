namespace DaoLayer.Persistence.Student
{
    public class StudentDao: IStudentDao
    {
        private readonly IApiServiceDao apiServiceDao;
        private readonly IDbResult dbResult;

        public StudentDao(IApiServiceDao apiServiceDao,IDbResult dbResult)
        {
            this.apiServiceDao = apiServiceDao;
            this.dbResult = dbResult;
        }


    }
}
