using System.Collections.Generic;
using System.Linq;

///<summary>
///Original: http://3dk.easycruit.com/vacancy/1161591/3032?iso=dk
///Their own repost: https://3dk.easycruit.com/vacancy/1385970/3032?iso=dk
///</summary>
namespace JobAd
{
    
  public class JobApplicant
  {
    private IList<Skill> _skillList = new List<Skill>();

    public string Name { get; set; }
    public string AdditionalMotivation { get; set; }
    public List<Skill> Skills
    {
    get
      {
        return _skillList;
      }
    }

    public const string ContactPersonAt3 = "Steffen JÃ¸rgensen, 31 200 953";

    /// <summary>
    /// Call to get applicant awesomeness level. The higher score the better.
    /// </summary>
    public int JobApplicantAwesomenessLevel()
    {
      int applicantAwesomenessLevel = 0;

      if (ApplicantHasRequiredSkills())
      {
        applicantAwesomenessLevel = 10; 
        applicantAwesomenessLevel += NiceToHaveAwesomenessLevel();
        applicantAwesomenessLevel += MetaAwesomenessLevel();
      }
      return applicantAwesomenessLevel;
    }

    private bool ApplicantHasRequiredSkills()
    {
      List<Skill> requiredSkills = new List<Skill> {
        new Skill() { SkillName = "ASP.NET", SkillLevel = SkillLevel.AVERAGE, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "ASP.NET MVC", SkillLevel = SkillLevel.AVERAGE, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "C#", SkillLevel = SkillLevel.AVERAGE, SkillCategory = SkillCategory.Programming }
      };
      foreach (Skill skill in requiredSkills)
      {
        if (HasProgrammingSkill(skill))
        {
          continue;
        }
        return false;
      }
      return true;
    }

    private bool HasProgrammingSkill(Skill skill)
    {
      var programmingSkills = RelevantSkills(SkillCategory.Programming);
      return programmingSkills.Where(x => x.SkillName == skill.SkillName && (x.SkillLevel == SkillLevel.Average || x.SkillLevel == SkillLevel.Pro).Any();
    }

    private int NiceToHaveAwesomenessLevel()
    {
      List<Skill> niceToHaveSkills = new List<Skill>() {
        new Skill() { SkillName = "Entity Framework", SkillLevel = SkillLevel.BEGINNER, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "SQL", SkillLevel = SkillLevel.BEGINNER, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "Unit testing", SkillLevel = SkillLevel.BEGINNER, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "Agile development (Scrum)", SkillLevel = SkillLevel.BEGINNER, SkillCategory = SkillCategory.Programming },
        new Skill() { SkillName = "HTML/CSS", SkillLevel = SkillLevel.BEGINNER, SkillCategory = SkillCategory.Programming }
      };
      return CountRelevantProgrammingSkills(niceToHaveSkills, SkillCategory.Programming);
    }

    private int MetaAwesomenessLevel()
    {
      List<Skill> metaSkills = new List<Skill>() {
        new Skill() { SkillName = "Enjoys fast paced environment", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
        new Skill() { SkillName = "Is studying computer science or equivalent education", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
        new Skill() { SkillName = "Curios about new skills and technology", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
        new Skill() { SkillName = "Enjoys working closely with project managers and colleagues", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
        new Skill() { SkillName = "Will work in small .NET team with senior and junior developers", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
        new Skill() { SkillName = "Can work 15 hours/week", SkillLevel = SkillLevel.AVERAGE, SkillCategory = "Meta" },
      };
      return CountRelevantProgrammingSkills(metaSkills, SkillCategory.Meta);
    }

    private int CountRelevantProgrammingSkills(IEnumerable<Skill> skillSet, SkillCategory category)
    {
      var relevantSkills = RelevantSkills(category);
      return skillSet
        .Join(relevantSkills,
          x => x.SkillName,
          x => x.SkillName,
          (x,y) => x)
        .Count();
    }

    private IEnumerable<Skill> RelevantSkills(SkillCategory category)
    {
      return Skills.Where(x => x.SkillCategory == category);
    }
  }

  public class Skill
  {
    public string SkillName { get; set; }
    public SkillCategory SkillCategory { get; set; }
    public SkillLevel SkillLevel { get; set; }
  }

  public enum SkillLevel
  {
    Beginner, Average, Pro
  }

  public enum SkillCategory
  {
    Programming, Meta
  }
}
