namespace G2Libsys.Data.Repository
{
	using Dapper;

	/// <summary>
	/// Required namespaces
	/// </summary>

	#region Namespaces

	using G2Libsys.Library;
	using System.Collections.Generic;
	using System.Data;
	using System.Threading.Tasks;

	#endregion Namespaces

	// Ändringar här måste även göras i IUserRepository

	/// <summary>
	/// User repository for implementation of specific queries
	/// </summary>
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public UserRepository() { }

		/// <summary>
		/// Insert new user and return ID
		/// </summary>
		/// <param name="item"></param>
		public override async Task<int> AddAsync(User item)
		{
			// Map item
			DynamicParameters parameters = new DynamicParameters();
			parameters.AddDynamicParams(item);

			// Create ID parameter for output
			parameters.Add("NewID",
					dbType: DbType.Int32,
				 direction: ParameterDirection.Output);

			// Open connection
			using IDbConnection _db = Connection;

			// Insert mapped item and set NewID to created item ID
			await _db.ExecuteScalarAsync<int>(
						sql: GetProcedureName<User>("insert"),
					  param: parameters,
				commandType: CommandType.StoredProcedure);

			// Return the ID of inserted item
			return parameters.Get<int>("NewID");
		}

		/// <summary>
		/// Example User specific query
		/// </summary>
		public async Task<User> VerifyLoginAsync(string email, string password)
		{
			using IDbConnection _db = base.Connection;

			// Fetch user with correct username and password
			return await _db.QueryFirstOrDefaultAsync<User>(
						sql: GetProcedureName<User>("verifylogin"),
					  param: new { email, password },
				commandType: CommandType.StoredProcedure);
		}

		/// <summary>
		/// Check if email already exist
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public async Task<bool> VerifyEmailAsync(string email)
		{
			using IDbConnection _db = base.Connection;

			// Return true if email exist in db
			return await _db.ExecuteScalarAsync<bool>(
						sql: GetProcedureName<User>("verifyemail"),
					  param: new { email },
				commandType: CommandType.StoredProcedure);
		}

		public async Task<IEnumerable<Loan>> GetLoansAsync(int id)
		{
			using IDbConnection _db = Connection;

			// Return all items of type T
			return await _db.QueryAsync<Loan>(
						sql: GetProcedureName<User>("getloans"),
					  param: new { id },
				commandType: CommandType.StoredProcedure);
		}

		public async Task<IEnumerable<LibraryObject>> GetLoanObjectsAsync(int id)
		{
			using IDbConnection _db = Connection;

			// Return all items of type T
			return await _db.QueryAsync<LibraryObject>(
						sql: GetProcedureName<User>("getloanobjects"),
					  param: new { id },
				commandType: CommandType.StoredProcedure);
		}
	}
}