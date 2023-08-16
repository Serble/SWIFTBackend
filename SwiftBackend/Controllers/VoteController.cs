using System.Text.RegularExpressions;
using GeneralPurposeLib;
using Microsoft.AspNetCore.Mvc;
using SwiftBackend.Schemas;

namespace SwiftBackend.Controllers; 

[ApiController]
[Route("vote")]
public class VoteController : ControllerBase {
    
    [HttpPost]
    public async Task<ActionResult> Vote([FromHeader] AuthenticationHeader authorization, [FromBody] VoteBody body) {
        SerbleUser serbleUser = await authorization.GetUser();
        if (serbleUser == null) {
            return Unauthorized();
        }
        SwiftUser user = await Program.StorageManager.GetUser(serbleUser.Id);
        if (user == null) {
            return BadRequest("User does not exist");
        }

        if (!new Regex(@"^([a-z0-9]+(-[a-z0-9]+)*\.)+[a-z]{2,}$").IsMatch(body.Domain)) {
            return BadRequest("Invalid domain");
        }

        int voteWeight = 1;
        if (user.Premium) {
            voteWeight = 2;
        }
        if (user.Admin) {
            voteWeight = 10;
        }
        UserVote vote = new() {
            UserId = user.Id,
            Domain = body.Domain,
            Vote = (body.Vote ? 1 : -1) * voteWeight
        };
        await Program.StorageManager.SubmitVote(vote);
        return Ok();
    }
    
    [HttpGet("{domain}")]
    public async Task<ActionResult<SiteVoteProfile>> GetVotes(string domain, [FromHeader] AuthenticationHeader authorization = null) {
        bool premium = false;
        string noPremiumReason = "No auth header";
        SiteVoteProfile profile = await Program.StorageManager.GetSiteVotes(domain);
        if (authorization != null) {  // If any auth request fails then just don't show the bot rating
            SerbleUser serbleUser = await authorization.GetUser();
            if (serbleUser != null) {
                SwiftUser user = await Program.StorageManager.GetUser(serbleUser.Id);
                if (user is {Premium: true}) {
                    noPremiumReason = "User is premium";
                    premium = true;
                    profile.BotRating = await Program.StorageManager.GetBotRating(domain);
                    if (profile.BotRating == -1) {  // -1 means unknown (It hasn't been rated yet)
                        profile.BotRating = await AiRateManager.GetBotRating(domain);  // So we try to get a rating from the AI
                    }
                }
                else {
                    noPremiumReason = "User is not premium or didn't auth";
                }
            }
            else {
                noPremiumReason = "SerbleUser not found";
            }
        }
        Logger.Debug("Result of domain :" + domain + " is " + profile.ToJson());
        Response.Headers.Add("X-Premium", "" + premium);
        Response.Headers.Add("X-NoPremiumReason", noPremiumReason);
        return Ok(profile);
    }
    
    [HttpGet("user/{domain}")]
    public async Task<IActionResult> GetUserVote([FromHeader] AuthenticationHeader authorization, string domain) {
        SerbleUser serbleUser = await authorization.GetUser();
        if (serbleUser == null) {
            return Unauthorized();
        }
        SwiftUser user = await Program.StorageManager.GetUser(serbleUser.Id);
        if (user == null) {
            return BadRequest("User does not exist");
        }
        int? vote = await Program.StorageManager.GetUserVote(user.Id, domain) ?? 0;
        return Ok(new {
            vote
        });
    }
    
    [HttpOptions]
    public IActionResult Options() {
        Response.Headers.Add("Allow", "POST, OPTIONS");
        return Ok();
    }
    
    [HttpOptions("{domain}")]
    public IActionResult OptionsDo() {
        Response.Headers.Add("Allow", "GET, OPTIONS");
        return Ok();
    }
    
    [HttpOptions("user/{domain}")]
    public IActionResult OptionsD() {
        Response.Headers.Add("Allow", "GET, OPTIONS");
        return Ok();
    }
    

}