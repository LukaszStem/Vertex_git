dirPath = "C:\Users\weirdluki21\Downloads\"
randomSeeds = [50, 100, 150, 200]
neuronIdSets = [1, 873]

% for i = 1:size(randomSeeds, 2)
%     fileName = strcat(dirPath, num2str(randomSeeds(i)), "_");
%     
%     % With stimulation
%     generateValues(true, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
%         strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "Stim"));
%     clearvars -except i dirPath randomSeeds neuronIdSets fileName
%     
%     % Without stimulation
%     generateValues(false, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
%         strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "NoStim"));
%     clearvars -except i dirPath randomSeeds neuronIdSets fileName
% end

numberOfPoints = 100;
resolution = 10000/numberOfPoints;

currentStartTime = 0;

stimVec = ["Stim", "NoStim"];

rates = zeros([2, size(randomSeeds, 2), numberOfPoints-1, 3]);

for i = 1:2
    for j = 1:size(randomSeeds, 2)
        fprintf("Starting anaylsis with i:%i, j:%i\n", i, j)
        
        fileName = strcat(dirPath, num2str(randomSeeds(j)), "_");
        resultsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), ".mat");
        connectionsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), "Connections.mat");
        load(resultsFile);
        load(connectionsFile);
        [rateOfChangeStim, avgPos] = ...
            calculatePlasticityValue(Results, 1:873, connections, numberOfPoints, 10000, 873);
        for y=1:numberOfPoints-1
            rates(i, j, y, 1) = rateOfChangeStim(y, end, 1);
            rates(i, j, y, 2) = rateOfChangeStim(y, end, 2);
            rates(i, j, y, 3) = rateOfChangeStim(y, end, 3);
        end
        clearvars Results connections
        
        fprintf("Finished anaylsis with i:%i, j:%i\n", i, j)
    end
end


%generateVertexVisualizerFolder('C:\Users\wassa\smalltest', Results, connections);

%[rateOfChangeStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

%[rateOfChangeNoStim, avgPos] = ...
%    calculatePlasticityValue(Results_Stim, 1:100, 1:1000, 873)

