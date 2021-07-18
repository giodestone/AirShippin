using System;

[Serializable]
public enum JobState
{
    NoJob,
    HasJob,
    SubmitJob,
    CancelJob,
    JobComplete
}