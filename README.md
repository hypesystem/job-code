Job listings are important.
They are the first interaction potential applicants have with your job.
This should be a serious encounter.

Unfortunately, some companies that focus on programming choose to write their listings as (sometimes compiling) code.
As if that wasn't lacking enough in seriousity, the code is also often bad.

This library has the goal of documenting, analysing, and---most of all---shaming these job ads.

Contributing
============

There are a couple of ways to contribute.
If you want to correct something, please make a PR or an issue---it's that easy.

Get the code by cloning:

    git clone git@github.com:hypesystem/job-code

If you want to add an example of job listing code, there are a couple of ways to go about it:

- Give us a hint by creating an issue.
  A link to the code in question, as well as a screenshot and maybe the code itself, is all that is needed.
- Add the example in a PR.
  Create a new folder `<company>/<date-seen-live>`, where `<date-seen-live>` is when you saw the code in the wild.
  This folder should contain `original.cs`, an image (screenshot of photo of the code) `screenshot.png`, as well as a description of the listing `README.md`.
  The description should detail where the listing was found, and what job it advertises.
- Do the analysis and refactoring.
  If you want to go further, and actually comment on the code you found, you can do that!
  Add the analysis of the code to the `README.md` file, and add the completely refactored code as `refactored.cs`.
