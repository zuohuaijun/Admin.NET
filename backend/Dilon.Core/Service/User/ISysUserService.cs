using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ISysUserService
    {
        Task AddUser(AddUserInput input);

        Task ChangeUserStatus(UpdateUserInput input);

        Task DeleteUser(DeleteUserInput input);

        Task<IActionResult> ExportUser();

        Task ImportUser(IFormFile file);

        Task<dynamic> GetUser(long id);

        Task<dynamic> GetUserById(long userId);

        Task<List<long>> GetUserDataScopeIdList();

        Task<List<long>> GetUserDataScopeIdList(long userId);

        Task<dynamic> GetUserOwnData([FromQuery] QueryUserInput input);

        Task<dynamic> GetUserOwnRole([FromQuery] QueryUserInput input);

        Task<dynamic> GetUserSelector([FromQuery] UserSelectorInput input);

        Task GrantUserData(UpdateUserInput input);

        Task GrantUserRole(UpdateUserInput input);

        Task<dynamic> QueryUserPageList([FromQuery] UserPageInput input);

        Task ResetUserPwd(QueryUserInput input);

        Task SaveAuthUserToUser(AuthUserInput authUser, CreateUserInput sysUser);

        Task UpdateAvatar(UploadAvatarInput input);

        Task UpdateUser(UpdateUserInput input);

        Task UpdateUserInfo(UpdateUserInput input);

        Task UpdateUserPwd(ChangePasswordUserInput input);
    }
}