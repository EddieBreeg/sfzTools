# sfzTools

SfzTools is a set of... tools, yes you guessed it, made to help you create SFZ instruments. If you don't know
what SFZ is, it is a sampler format which has the great advantage if being free and open, contrary to formats like
Kontakt instruments. And: it is crossplatform. The issue being that it is no longer developed, and that there is almost no tool to actually CREATE sfz files, which are basically a text file containing the "source code" of your instrument...
As much as it is good for being free, it is not exactly the easiest format to handle since you have to do everything by hand... or do you?

The idea behind sfzTools is to provide you with programs which automate the sometimes painful process that is
writing an sfz file. Those tools are available on Windows, Linux and Mac, please refer to the [Releases](https://github.com/EddieBreeg/sfzTools/releases) page to download the file containing the executables. And of course you also get the source code if you want to modify the programs according to your needs or even contribute (which I would of course recommend assuming you know a bit of C# programming)

**Development dependencies**: EddieBreeg.CoreExtensions

## autoRename

Let's assume that you have recorded samples from an instrument, a piano for example. You would have several velocity layers, round robins, release triggers... Which represents quite a lot of files. Usually you'd want to name those files accordingly but quite frankly, it is not a fun task, nor is it really interesting. **autoRename** allows you to rename all those files automatically. Let's assume that you have a well organized folders with your audio files sorted in different subdirectories, for example:
```
Example
├───level1
│   ├───RR1
│   │       sample1.wav
│   │       sample2.wav
│   │
│   └───RR2
│           sample1.wav
│           sample2.wav
│
└───level2
    ├───RR1
    │       sample1.wav
    │       sample2.wav
    │
    └───RR2
            sample1.wav
            sample2.wav
```
You don't want to take care of all of this yourself do you? I mean, you can but... what's the point?
Start by running the program, there is a whole section about that below if you need help.
Then it will ask you for the path. What you want to enter there is the path to the root folder containing all the samples (here, the Example folder). And free little tip: if you drag and drop the folder in the console it will write the path for you, there you go.

Next! The first note, meaning: what is the note contained in the first sample (in this example, what the note of *sample1.wav* )? The program assumes it is the same for every subfolder!
If not, you still can run the program on the folder that is different from the other ones.
The default is C0, so if you don't enter any value, that's what the program will take as a value.

The interval: what interval (in semitones) separate samples from each other. The default is 5, which corresponds to an instrument you would have sampled using cycles of fourths. Again the program assumes it will be the same for every subfolder, and also assume it is consistent across all the samples in the folder.

Finally the file extension, default being wav. DO NOT put a `.` before the extension. This parameter ensures that the program only takes the samples into account and ignores everything else. You never know when you could have some random files laying down in your folders right?

For the sake of this example I'll assume we left everything as default except the first note (here I put C1). The program will rename all the samples according to the folder they're in and the note they correspond to. For our case the result would then be:
```
Example
├───level1
│   ├───RR1
│   │       C1_level1_RR1.wav
│   │       F1_level1_RR1.wav
│   │
│   └───RR2
│           C1_level1_RR2.wav
│           F1_level1_RR2.wav
│
└───level2
    ├───RR1
    │       C1_level2_RR1.wav
    │       F1_level2_RR1.wav
    │
    └───RR2
            C1_level2_RR2.wav
            F1_level2_RR2.wav
```
And that's pretty much all you have to know about **autoRename**!

## filenameParser

Okay so you now have all your samples renamed, nice and clean. But you still have to write
the SFZ file. If you're here you probably know how to do that but just in case you don't, follow [this link](https://sfzformat.com/). All you need to know is there! With that out of the way let's dive into it! Let's take the last example where we left it (see above).
We don't want to map all those samples by hand do we? So now, run **filenameParser**.

Again, it will ask for a path, enter the path to the folder which contains all the samples. Here, it's called *Example/*.

Next: the extension. Same exact thing as for **autoRename**, don't put any `.`, and the default is wav.

Then, the separator character. The program searches for pieces of information (called tokens) in the filenames, but first it needs to know how to find those! By default it uses `_` to parse the filenames since it's what **autoRename** uses, but you can provide pretty much any other character you want, even a space.

The parser will then analyze filenames to find tokens. It will show you the tokens that were found and will ask you what you want to do with them. For each token there are 3 options:
- 0 = the token will be completely be ignored
- 1 = the token is considered to be a note name
- 2 = the token will be used to make group names

Normally the program automatically finds the note name, and puts the default value at 2 for everything else.
Note that every samples MUST have at least the note name for the program to work.\
Also note that, if loading samples with different amounts of tokens can work, I wouldn't recommend it. If you do it anyway, I'd advise you to make sure that the note name is at the start everywhere so that the program can find it. It could be useful for things like release triggers for example.

Finally, the stretch mode describes how the samples should be stretched across the keyboard.
- 0 = No stretch: each sample will only be mapped to the corresponding key
- 1 = Stretch down (default): each sample will be stretch down until it reaches the next sample below it.
- 2 = Stretch up: each sample will be stretch up until it reaches the next sample above it.

And that's all you have to do! The program will prompt you with output path. The .sfz file is created in the parent folder to the one you have provided. All samples will automatically be moved to the provided root folder. The subdirectories won't be changed.

Here is the resulting folder structure when we apply this program on our example:
```
map.sfz
Example/
│   C1_level1_RR1.wav
│   C1_level1_RR2.wav
│   C1_level2_RR1.wav
│   C1_level2_RR2.wav
│   F1_level1_RR1.wav
│   F1_level1_RR2.wav
│   F1_level2_RR1.wav
│   F1_level2_RR2.wav
│
├───level1
│   ├───RR1
│   └───RR2
└───level2
    ├───RR1
    └───RR2
```
And here is the sfz code:
```sfz
<control>
default_path=Example/

<group> //level1 RR1
	<region>
	 sample=C1_level1_RR1.wav
	 pitch_keycenter=C1
	 lokey=12
	 hikey=12

	<region>
	 sample=F1_level1_RR1.wav
	 pitch_keycenter=F1
	 lokey=13
	 hikey=17

<group> //level1 RR2
	<region>
	 sample=C1_level1_RR2.wav
	 pitch_keycenter=C1
	 lokey=12
	 hikey=12

	<region>
	 sample=F1_level1_RR2.wav
	 pitch_keycenter=F1
	 lokey=13
	 hikey=17

<group> //level2 RR1
	<region>
	 sample=C1_level2_RR1.wav
	 pitch_keycenter=C1
	 lokey=12
	 hikey=12

	<region>
	 sample=F1_level2_RR1.wav
	 pitch_keycenter=F1
	 lokey=13
	 hikey=17

<group> //level2 RR2
	<region>
	 sample=C1_level2_RR2.wav
	 pitch_keycenter=C1
	 lokey=12
	 hikey=12

	<region>
	 sample=F1_level2_RR2.wav
	 pitch_keycenter=F1
	 lokey=13
	 hikey=17
```

As you can see the program doesn't generate velocity values, or round robins... those are things you still have to do yourself unfortunately. But still, hopefully this little program should make you gain a ton of time.

## autoBuilder

OK fine you have program to automatically rename your sample, and you have another one to 
map them... but what if you could do both at the same time? That's right there is another tool
for that too! **autoBuilder** is essentially a fusion of the two other tools, and even more. Let's get right in, by taking a new example.

Let's say we have exported all our samples to a folder, as follows:
```
Example/
|	file0.wav
|	file1.wav
|	file2.wav
|	file3.wav
|	file4.wav
|	file5.wav
|	file6.wav
|	file7.wav
```
Here we have an instrument with 1 articulation, 2 dynamic layers, 2 round robins, and 2 notes per group.

Those files hopefully are sorted using a specific hierarchy, for example:
```
└───articulation
    └───dynamicLevel
        └───round robin
                note
```
It's important that you know what hierarchy you have used, we'll get back to that in a minute.
Anyway, run autoBuilder. As usual, it will ask for the path and the file extension to look at (default is "wav").

Then, it will ask you for how many articulations you want to create, and what the names for each for each should be.
Note that the default number is 1, and that there is also a default name for each articulation (which will be prompted to you).
The same pieces of information will be asked for the dynamic levels and the round robins respectively. 

Next you will be asked for how many different notes there are in each group. It corresponds to how many separate notes you have recorded when you sampled the instrument. In our example, it's two.

Then, indicate what the first note is (default is C0), and what interval separates each note from the next (default is 5 semitones). Note that the program assumes those values are the same for every group!

Then comes the stretch mode:
- 0 = no strech mode, each sample will only be assigned to its root note
- 1 = each sample will be stretched to the note below
- 2 = each sample will be stretched to the note above

Finally, the hierarchy! Remember when I told you it was important? There it comes! The program needs to know in what order you have sorted your files. In our example we have use the following hierarchy: `articulation>dynamic level>round robin>note`
Use the corresponding value for each part of the hierarchy:
- 0 = articulation
- 1 = dynamic level
- 2 = round robin
- 3 = note name

Enter the values separated by a space. In our case it would be: "0 1 2 3" (which is the default).
But if your hierarchy is `articulation>dynamic level>note>round robin` for example, then you should enter "0 1 3 2"

And that's it! The program will rename the samples, and generate a .SFZ file with everything mapped according to the pieces of information you have provided. It create default velocity values as well as round robin sequences.
Note that the program computes how many samples there are based on the number of notes per group (which is why this information was needed) and the total number of groups. If the program finds out there are some remaining samples in the folder, it will add them into a separate group automatically. This can be useful for things like release triggers.

### Nota bene: how to run these programs on your operating system?

#### Windows
Seriously, it couldn't be easier: double click it.

#### Linux
The Linux versions are provided with .desktop files to make things easier. Simply double click the .desktop file. You will probably be prompted with a warning telling you the software is not trusted... fair enough I guess. You have the source code available anyway, so nothing to be scared of. Simply confirm and it *should* work. Make sure the desktop file stays in the same directory as the executable, otherwise you will have to edit the shortcut to point to the new location.

#### Mac
Similarly as for the Linux versions, the MacOS versions come with .command files you can double click to run the programs. If the .command file is not in the same folder as the executable, you'll need to edit it to update the path.
