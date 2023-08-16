using Microsoft.AspNetCore.Mvc;

namespace SwiftBackend.Schemas;

public class AuthenticationHeader {

    [FromHeader] public string Authorization { get; set; } = null!;

    public async Task<SerbleUser> GetUser() {
        if (Program.AllowTestUser) {
            if (Authorization == "Bearer test") {
                return new SerbleUser {
                    Id = "test",
                    Username = "test",
                };
            }
        }

        try {
            string token = Authorization.Replace("Bearer ", "");
            SerbleApi api = new(token);
            SerbleUser user = await api.GetUser();
            if (user == null) {
                return null;
            }

            return await Program.StorageManager.GetUser(user.Id) == null ? null : user;
        }
        catch (Exception) {
            return null;
        }
    }

}