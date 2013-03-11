namespace Knet.Phone.Client.ArcGIS.Tasks
{
    public class UpdateAttachmentParameters
    {
        public int FeatureId { get; set; }

        public int AttachmentId { get; set; }

        public string Name { get; set; }

        public byte[] Attachment { get; set; }
    }
}