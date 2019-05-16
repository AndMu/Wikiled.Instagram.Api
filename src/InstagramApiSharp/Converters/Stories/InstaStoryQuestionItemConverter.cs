using System;

namespace Wikiled.Instagram.Api.Converters.Stories
{
    internal class InstaStoryQuestionItemConverter : IObjectConverter<InstaStoryQuestionItem, InstaStoryQuestionItemResponse>
    {
        public InstaStoryQuestionItemResponse SourceObject { get; set; }

        public InstaStoryQuestionItem Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var QuestionItem = new InstaStoryQuestionItem
                               {
                                   Height = SourceObject.Height,
                                   IsHidden = SourceObject.IsHidden,
                                   IsPinned = SourceObject.IsPinned,
                                   Rotation = SourceObject.Rotation,
                                   Width = SourceObject.Width,
                                   X = SourceObject.X,
                                   Y = SourceObject.Y,
                                   Z = SourceObject.Z
                               };
            QuestionItem.QuestionSticker = ConvertersFabric.Instance.GetStoryQuestionStickerItemConverter(SourceObject.QuestionSticker).Convert();
            return QuestionItem;
        }
    }
}
