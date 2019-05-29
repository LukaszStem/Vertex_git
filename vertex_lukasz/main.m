dirPath = "C:\Users\wassa\VertexOutput"
% randomSeeds = [50, 100, 150, 200]
randomSeeds = [50]
neuronIdSets = [1, 100]

for i = 1:size(randomSeeds, 2)
    fileName = strcat(dirPath, num2str(randomSeeds(i)), "_");
    
    % With stimulation
    generateValues(true, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
        strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "Stim"));
    clearvars -except i dirPath randomSeeds neuronIdSets fileName
    
    % Without stimulation
    generateValues(false, neuronIdSets(1):neuronIdSets(2), randomSeeds(i), ...
        strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), "NoStim"));
    clearvars -except i dirPath randomSeeds neuronIdSets fileName
end

% numberOfPoints = 10000;
% resolution = 10000/numberOfPoints;
% 
% currentStartTime = 0;
% 
% stimVec = ["Stim", "NoStim"];
% 
% rates = zeros([2, size(randomSeeds, 2), numberOfPoints-1, 3]);
% 
% for i = 1:2
%     for j = 1:size(randomSeeds, 2)
%         fprintf("Starting anaylsis with i:%i, j:%i\n", i, j)
%         
%         fileName = strcat(dirPath, num2str(randomSeeds(j)), "_");
%         resultsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), ".mat");
%         connectionsFile = strcat(fileName, num2str(neuronIdSets(1)), "-", num2str(neuronIdSets(2)), stimVec(i), "Connections.mat");
%         load(resultsFile);
%         load(connectionsFile);
%         [rateOfChangeStim, avgPos] = ...
%             calculatePlasticityValue(Results, 1:873, connections, numberOfPoints, 10000, 873);
%         for y=1:numberOfPoints-1
%             rates(i, j, y, 1) = rateOfChangeStim(y, end, 1); %Summed synaptic increment
%             rates(i, j, y, 2) = rateOfChangeStim(y, end, 2); %Summed synaptic decrement
%             rates(i, j, y, 3) = rateOfChangeStim(y, end, 3);
%         end
%         clearvars Results connections
%         
%         fprintf("Finished anaylsis with i:%i, j:%i\n", i, j)
%     end
% end

% %% plot values
% close all;
% start = 50;
% x = start:numberOfPoints-1;
% increments = zeros([1, (numberOfPoints-start)]);
% decrements = zeros([1, (numberOfPoints-start)]);
% finalValues = zeros([1, (numberOfPoints-start)]);
% for i=1:2
%     for y= start:numberOfPoints-1
%         tmpIncrement = 0;
%         tmpDecrement = 0;
%         tmpFinal = 0;
%         for j = 1:4
%            tmpIncrement = tmpIncrement + rates(i, j, y, 1);
%            tmpDecrement = tmpDecrement + rates(i, j, y, 2);
%            tmpFinal = tmpFinal + rates(i, j, y, 3);
%         end
%         indx = y-start+1;
%         increments(indx) = tmpIncrement/4;
%         decrements(indx) = tmpDecrement/4;
%         if indx > 1
%             finalValues(indx) = tmpFinal/4 + finalValues(indx-1);
%         else
%             finalValues(indx) = tmpFinal/4;
%         end
%     end
%     figure;
%     yyaxis left;
%     plot(x, increments, 'r-');
%     ylabel('Average instantaneous change');
%     hold on;
%     plot(x, decrements, 'b-');
%     %ylim([-0.03 0.06])
%     ylim([-0.1 0.2])
%     yyaxis right
%     plot(x, finalValues)
%     
%     set(gcf,'Color','w')
%     ylabel('Summed change')
%     % Add title and x axis label
%     xlabel('Time in milliseconds')
%     ylim([0 17])
%     if i == 1
%         title('STDP with tDCS')
%     else
%         title('STDP without tDCS')
%     end
% end






