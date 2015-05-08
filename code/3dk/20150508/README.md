The Danish branch of the telecommunications provider, 3, has used this code for more than one job listing.
Here are some URLs that will probably break in the fututre, where it was used:

- https://3dk.easycruit.com/vacancy/1385970/3032?iso=dk (live 2015-05-08)
- http://3dk.easycruit.com/vacancy/1161591/3032?iso=dk (live 2014-03-28)

The job listing was for a .NET student developer, and the code was---fittingly---written in C#.

![Screenshot of job listing site](screenshot.png?raw=true)

Below is a review of the code.
The original can be read [here](original.cs).
The refactored code can be read [here](refactored.cs).

-----

## Using/namespace

The namespace and using statements seem to be switched up a bit in this code.
There can be several namespaces in a single file, but using statements affect what is accessible in the whole file.
Hence, using files are traditionally placed in the beginning, outside of namespaces:

    using System.Collections.Generic;
    using System.Linq;

    namespace JobAd

## Properties

One of C#'s neater features is the existence of properties, which make getters and setters obsolete, and allow for nice control of access of internals.
In the 3 code we see them used nicely, however the `Skills` property leaks implementation.
You should never leak a list in your code, unless absolutely necessary.
In this case it isn't.
I would rewrite the property without a setter, using a private internal field:

    private IList<Skill> _skillList = new List<Skill>();

    public IEnumerable<Skill> Skills
    {
        get
        {
            return _skillList;
        }
    }

This hides the implementation, leaking only an enumerable.

## Unnecessary variables

In the JobApplicationAwesomenessLevel method, we see an unnecessary variable, `isQualified`.
This variable doesn't actually make the code more easily readable. Consider the two:

    bool isQualified = ApplicantHasRequiredSkills();
    if (isQualified)

... versus ...

    if (ApplicantHasRequiredSkills())

The latter is much closer to natural language and more easily readable, and thus, preferable.

Further on, in ApplicantHasRequiredSkills, we see the variable `hasSkill`, which uses an inline LINQ query.
It would be much more readable if this statement had been moved out to a different method.
This, in turn, would make the `hasSkill` variable obsolete:

        if (HasSkill(skill))
        {
            continue;
        }

In the CountNiceToHaveSkills method, we see an entirely unused parameter.
This should be removed.

## Repeated code

The code that returns the number of relevant programming skills is repeated between MetaSkillBonus and CountNiceToHaveSkills.
This should be extracted into its own method.

## String constants

A couple of string constants are used.
This should generally be avoided. SkillCategory could be an enum or otherwise simply an object.
It should not be a bare string, as anything (many invalid things) can be passed, and the error will not be caught before this string is actually used.

## Naming

CountNiceToHaveSkills is named inconsistently with the other methods gathering AwesomenessLevel-points.
Using a consistent language is very important as it reduces confusion.
A name that indicated the currency of AwesomenessLevel might be preferable.
For example, the methods could be named `NiceToHaveAwesomenessLevel` and `MetaAwesomenessLevel`.

The enums used are ALL CAPS.
There is no reason for this.
This is quite simply a remainder from back in the days where one was forced to use integer constants, and should be avoided.

## LINQ

Personally I do not much care for the LINQ syntax used here.
I think it is a horrible idea to mix different types of syntax in the same code.
Instead, I would use the LINQ syntax that conforms to C# code standards:

    var relevantSkills = RelevantSkills("Programming");
    return niceToHaveSkills
        .Join(relevantSkills,
            x => x.SkillName,
            x => x.SkillName,
            (x,y) => x)
        .Count();

The way LINQ is used in the 3 code results in the `this` keyword being necessary.
I would argue that writing code there `this` is necessary means that you are doing something wrong (though, depending on the convention used by your team, constructors are allowed to use the keyword).
