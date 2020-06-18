using ClassLibrary;
using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleCloudVision
{
    public class VisionClient
    {
        private readonly ImageAnnotatorClient _client;

        private static readonly List<Feature> defaultFeatures = new List<Feature>();
        private static readonly List<string> acceptedAnimals = new List<string>();

        static VisionClient()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", Constants.CREDENTIALS_FILE);
            
            defaultFeatures.Add(new Feature { Type = Feature.Types.Type.LogoDetection });
            defaultFeatures.Add(new Feature { Type = Feature.Types.Type.FaceDetection });
            defaultFeatures.Add(new Feature { Type = Feature.Types.Type.LabelDetection });
            defaultFeatures.Add(new Feature { Type = Feature.Types.Type.SafeSearchDetection });

            acceptedAnimals.Add("MAMMAL");
            acceptedAnimals.Add("BIRD");
            acceptedAnimals.Add("REPTILE");
            acceptedAnimals.Add("AMPHIBIAN");
            acceptedAnimals.Add("FISH");
            acceptedAnimals.Add("DOG");
            acceptedAnimals.Add("CAT");
        }

        public VisionClient()
        {
            _client = ImageAnnotatorClient.Create();
        }

        public Response AnalyzeImage(Image image, float minConfidence)
        {
            try
            {
                var annotate = new AnnotateImageRequest();
                annotate.Image = image;
                annotate.Features.AddRange(defaultFeatures);

                var response = _client.Annotate(annotate);

                bool isSafe = CheckSafeSearch(response);
                bool containsLogos = CheckLogos(response, minConfidence);

                bool containsPeople = false;
                bool containsAnimals = false;

                var labelList = response.LabelAnnotations.Where(x => x.Score >= minConfidence);

                foreach (var label in labelList)
                {
                    string descr = label.Description.ToUpper();

                    if (descr.Equals("PEOPLE"))
                    {
                        containsPeople = true;
                    }

                    else if (acceptedAnimals.Contains(descr))
                    {
                        containsAnimals = true;
                    }
                }

                containsPeople = containsPeople || (response.FaceAnnotations.Count > 0);

                bool Success = isSafe && !containsLogos && !containsPeople && containsAnimals;

                if (Success)
                {
                    return new Response
                    {
                        Sucesso = true,
                        ContemAnimal = containsAnimals,
                        ContemLogomarca = containsLogos,
                        ContemPessoa = containsPeople,
                        ConteudoSeguro = isSafe
                    };
                }
                else
                {
                    return new Response
                    {
                        Sucesso = false,
                        Descricao = "A imagem não é adequada.",
                        ContemAnimal = containsAnimals,
                        ContemLogomarca = containsLogos,
                        ContemPessoa = containsPeople,
                        ConteudoSeguro = isSafe
                    };
                }
            }

            catch (Exception e)
            {
                return new Response
                {
                    Sucesso = false,
                    Descricao = string.Format("Ocorreu um erro ao tentar analisar a imagem ({0}).", e.Message)
                };
            }
        }



        private bool CheckSafeSearch(AnnotateImageResponse response)
        {
            var safeSearch = response.SafeSearchAnnotation;

            bool isAdult = safeSearch.Adult != Likelihood.VeryUnlikely && safeSearch.Adult != Likelihood.Unlikely;
            bool isViolent = safeSearch.Violence != Likelihood.VeryUnlikely && safeSearch.Violence != Likelihood.Unlikely;
            bool isRacy = safeSearch.Racy != Likelihood.VeryUnlikely && safeSearch.Racy != Likelihood.Unlikely;

            return !(isAdult || isViolent || isRacy);
        }

        private bool CheckLogos(AnnotateImageResponse response, float minConfidence)
        {
            var list = response.LogoAnnotations.Where(x => x.Score >= minConfidence);

            return list.Count() > 0;
        }
    }
}
