using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Admin.NET.Core.Service
{
    public interface ISysUserService
    {
        Task AddUser(AddUserInput input);

        Task ChangeUserStatus(UpdateUserStatusInput input);

        Task DeleteUser(DeleteUserInput input);

        Task<IActionResult> ExportUser();

        Task<dynamic> GetUser(long id);

        Task<dynamic> GetUserById(long userId);

        Task<List<long>> GetUserDataScopeIdList();

        Task<List<long>> GetUserDataScopeIdList(long userId);

        Task<dynamic> GetUserOwnData([FromQuery] QueryUserInput input);

        Task<dynamic> GetUserOwnRole([FromQuery] QueryUserInput input);

        Task<dynamic> GetUserSelector([FromQuery] UserSelectorInput input);

        Task GrantUserData(UpdateUserRoleDataInput input);

        Task GrantUserRole(UpdateUserRoleDataInput input);

        Task ImportUser(IFormFile file);

        Task<dynamic> QueryUserPageList([FromQuery] UserPageInput input);

        Task ResetUserPwd(QueryUserInput input);

        Task SaveAuthUserToUser(AuthUserInput authUser, CreateUserInput sysUser);

        Task UpdateAvatar(UploadAvatarInput input);

        Task UpdateUser(UpdateUserInput input);

        Task UpdateUserInfo(UpdateUserBaseInfoInput input);

        Task UpdateUserPwd(ChangePasswordUserInput input);
    }
}