using BankWebAPI.Data;
using BankWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController : Controller
    {
        [HttpGet]
        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> profiles = DBManager.GetAllUserProfiles();
            return profiles;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetUserProfileById(int id)
        {
            var profile = DBManager.GetUserProfileById(id);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpGet]
        [Route("email/{email}")]
        public IActionResult GetUserProfileByEmail(string email)
        {
            var profile = DBManager.GetUserProfileByEmail(email);
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPost]
        public IActionResult CreateUserProfile([FromBody] UserProfile profile)
        {
            if (DBManager.InsertUserProfile(profile))
                return Ok("Successfully inserted");
            return BadRequest("Error in data insertion");
        }

        //[HttpPut("{id}")]
        [HttpPut]
        public IActionResult UpdateUserProfile([FromBody] UserProfile profile)
        {
            if (DBManager.UpdateUserProfile(profile))
                return Ok("Successfully updated");
            return BadRequest("Could not update");
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUserProfile(string id)
        {
            if (DBManager.DeleteUserProfile(id))
                return Ok("Successfully Deleted");
            return NotFound("Could not update");
        }
    }
}
