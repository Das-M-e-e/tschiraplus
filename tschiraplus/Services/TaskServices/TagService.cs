using System.Collections.ObjectModel;
using Core.Models;
using Services.DTOs;

namespace Services.TaskServices;

public class TagService
{
    public TagModel ConvertTagDtoToModel(TagDto tagDto)
    {
        TagModel result = new TagModel(); 
        result.TagId = tagDto.TagId;
        result.Title = tagDto.Title;
        result.ProjectId = tagDto.ProjectId;
        result.Description = tagDto.Description;
        result.ColorCode = tagDto.ColorCode;
        return result;
    }

    public List<TagModel> ApplyTags(List<TagDto> Selected_Tags)
    {
        List<TagModel> Result;
        Result = new List<TagModel>();
        foreach (TagDto Tag in Selected_Tags)
        {
            Result.Add(ConvertTagDtoToModel(Tag));
        }
        return Result;
    }

}