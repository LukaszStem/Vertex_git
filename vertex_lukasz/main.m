dirPath = "directory path"
randomSeeds = [50, 100, 150, 200]
neuronIdSets = [1, 436, 437, 873]

%generateValues( stimulationEnabled, preNeuronIds, randomSeed, fileName )

for i = 1:size(randomSeeds, 2)
    fileName = strcat(dirPath, num2str(randomSeeds(i)), "_");
    generateValues(false, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
        strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2))));
    clearvars -except dirPath randomSeeds neuronIdSets filename
    generateValues(false, neuronIdSets(3):neuronIdSets(4), randomSeeds(i), ...
        strcat(fileName, num2str(neuronIdSets(3)), "-", num2str(neuronIdSets(4))));
    clearvars -except dirPath randomSeeds neuronIdSets filename
end


%generateVertexVisualizerFolder('C:\Users\wassa\smalltest', Results, connections);

%[rateOfChangeStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

%[rateOfChangeNoStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

