using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using StreamDeck_KeePass.Domain.Settings;

namespace streamdeck_keepass.Domain
{
    internal class KeePassGenerate : IKeePassAction
    {
        private readonly GenerateSettings settings;

        public KeePassGenerate(object objectSettings) => settings = objectSettings as GenerateSettings;

        public string Invoke()
        {
            var profile = new PwProfile { CharSet = new PwCharSet() };
            profile.CharSet.Clear();

            if (settings.UseLowerCase) profile.CharSet.AddCharSet('l');
            if (settings.UseUpperCase) profile.CharSet.AddCharSet('u');
            if (settings.UseDigits) profile.CharSet.AddCharSet('d');
            if (settings.UsePunctuation) profile.CharSet.AddCharSet('p');
            if (settings.UseBrackets) profile.CharSet.AddCharSet('b');
            if (settings.UseSpecial) profile.CharSet.AddCharSet('s');

            profile.ExcludeLookAlike = settings.ExcludeLookAlike;
            profile.Length = (uint)settings.Length;
            profile.NoRepeatingCharacters = settings.MustOccurAtMostOnce;
            profile.ExcludeCharacters = settings.ExcludeCharacters;

            if (!string.IsNullOrEmpty(settings.CustomPattern))
            {
                profile.GeneratorType = PasswordGeneratorType.Pattern;
                profile.PatternPermutePassword = settings.RandomlyPermute;
                profile.Pattern = settings.CustomPattern;
            }

            PwGenerator.Generate(out ProtectedString pw, profile, null, new CustomPwGeneratorPool());

            return pw.IsEmpty ? string.Empty : pw.ReadString();
        }
    }
}

