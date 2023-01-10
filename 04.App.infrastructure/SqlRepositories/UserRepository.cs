using App.Domain;
using App.Domain.Contracts;

namespace _04.App.infrastructure.SqlRepositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IUnitOfWork unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var query = "insert into Users ([Name], [Age], [Credit], [CreatedTime]) output INSERTED.ID values (@Name, @Age, @Credit, @CreatedTime)";

            var parameters = new Dictionary<string, string>();

            parameters.Add("Name", user.Name);
            parameters.Add("Age", user.Age.ToString());
            parameters.Add("Credit", user.Credit.ToString());
            parameters.Add("CreatedTime", user.CreatedTime.ToString());
            var id = await unitOfWork.ExecuteScalarQueryAync<int>(query, parameters, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);


            user.Id = id;
            return user;
        }

        public async Task<User> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            var query = "Delete from users where id = @ID";
            var parameters = new Dictionary<string, string>();

            parameters.Add("ID", user.Id.ToString());
            var id = await unitOfWork.ExecuteScalarQueryAync<int?>(query, parameters, cancellationToken);

             await unitOfWork.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var query = "UPDATE Users SET [Name] = @Name, [Age] = @Age Where Id = @Id;";

            var parameters = new Dictionary<string, string>();

            parameters.Add("ID", user.Id.ToString());
            parameters.Add("Age", user.Age.ToString());
            parameters.Add("Name", user.Name);

            var id = await unitOfWork.ExecuteScalarQueryAync<int?>(query, parameters, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return user;
        }
    }
}
