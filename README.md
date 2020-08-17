# sfzTools

SfzTools is a set of... tools, yes you guessed it, made to help you create SFZ instruments. If you don't know
what SFZ is, it is a sampler format which has the great advantage if being free and open, contrary to formats like
Kontakt instruments. And: it is crossplatform. The issue being that it is no longer developed, and that there is almost no tool to actually CREATE sfz files, which are basically a text file containing the "source code" of your instrument...
As much as it is good for being free, it is not exactly the easiest format to handle since you have to do everything by hand... or do you?

The idea behind sfzTools is to provide you with programs which automate the sometimes painful process that is
writing an sfz file. Those tools are available on Windows, Linux and Mac, please refer to the [Releases](https://github.com/EddieBreeg/sfzTools/releases) page to download the file containing the executables. And of course you also get the source code if you want to modify the programs according to your needs or even contribute (which I would of course recommend assuming you know a bit of C# programming)

## autoRename

Let's assume that you have recorded samples from an instrument, a piano for example. You would have several velocity layers, round robins, release triggers... Which represents quite a lot of files. Usually you'd want to name those files accordingly but quite frankly, it is not a fun task, nor is it really interesting. **autoRename** allows you to rename all those files automatically. Let's assume that you have a well organized folders with your audio files sorted in different subdirectories, for example:
```
root
├───level1
│   ├───RR1
│   │       sample1
│   │       sample2
│   │       sample3
│   │       ...
│   ├───RR2
│   │       sample1
│   │       sample2
│   │       sample3
│   │       ...         
│   └───RR3
│           sample1
│           sample2
│           sample3
│           ...
├───level2
│   ├───RR1
│   │       sample1
│   │       sample2
│   │       sample3
│   │       ...
│   ├───RR2
│   │       sample1
│   │       sample2
│   │       sample3
│   │       ...
│   └───RR3
│           sample1
│           sample2
│           sample3
│           ...
└───level3
    ├───RR1
    │       sample1
    │       sample2
    │       sample3
    │       ...
    ├───RR2
    │       sample1
    │       sample2
    │       sample3
    │       ...
    └───RR3
            sample1
            sample2
            sample3
            ...
```
You don't want to take care of all of this yourself do you? I mean, you can but... what's the point?
Start by running the program, there is a whole section about that below if you need help.
Then it will ask you for the path. What you want to enter there is the path to the root folder. And free little tip: if you drag and drop the folder in the console it will write the path for you, there you go.

Next! The first note, meaning: what is the note contained in the first sample (in this example, what the note of *sample1*)? The program assumes it is the same for every subfolder!
If not, you still can run the program on the folder that is different from the other ones.
The default is C0, so if you don't enter any value, that's what the program will take as a value.

The interval: what interval (in semitones) separate samples from each other. The default is 5, which corresponds to an instrument you would have sampled using cycles of fourths. Again the program assumes it will be the same for every subfolder, and also assume it is consistent across all the samples in the folder.

Finally the file extension, default being wav. DO NOT put a `.` before the extension. This parameter ensures that the program only takes the samples into account and ignores everything else. You never know when you could have some random files laying down in your folders right?

For the sake of this example I'll assume we left everything as default. The program will rename all the samples according to the folder they're in and the note they correspond to. For our case the result would then be:
```
root
├───level1
│   ├───RR1
│   │       level1_RR1_C0.wav
│   │       level1_RR1_F0.wav
│   │       level1_RR1_A#0.wav
│   │       ...
│   ├───RR2
│   │       level1_RR2_C0.wav
│   │       level1_RR2_F0.wav
│   │       level1_RR2_A#0.wav
│   │       ...         
│   └───RR3
│           level1_RR3_C0.wav
│           level1_RR3_F0.wav
│           level1_RR3_A#0.wav
│           ...
├───level2
│   ├───RR1
│   │       level2_RR1_C0.wav
│   │       level2_RR1_F0.wav
│   │       level2_RR1_A#0.wav
│   │       ...
│   ├───RR2
│   │       level2_RR2_C0.wav
│   │       level2_RR2_F0.wav
│   │       level2_RR2_A#0.wav
│   │       ...         
│   └───RR3
│           level2_RR3_C0.wav
│           level2_RR3_F0.wav
│           level2_RR3_A#0.wav
│           ...
└───level3
    ├───RR1
    │       level3_RR1_C0.wav
    │       level3_RR1_F0.wav
    │       level3_RR1_A#0.wav
    │       ...
    ├───RR2
    │       level3_RR2_C0.wav
    │       level3_RR2_F0.wav
    │       level3_RR2_A#0.wav
    │       ...         
    └───RR3
            level3_RR3_C0.wav
            level3_RR3_F0.wav
            level3_RR3_A#0.wav
            ...
```
And that's pretty much all you have to know about **autoRename**!
