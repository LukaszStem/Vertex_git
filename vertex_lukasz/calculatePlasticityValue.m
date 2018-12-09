function [rateOfSynapticChange, averagePosition] = calculatePlasticityValue(results, neuronIds, connections, numberOfPoints, timeRange, layerBound) %Possibly add connectionDistance percentage? 

%Get average position of somas
positions = results.params.TissueParams.somaPositionMat(neuronIds, :);
averagePosition = [mean(positions(:, 1)), mean(positions(:, 2)), mean(positions(:, 3))];
resolution = timeRange/numberOfPoints;

%Get appropriate weights
weights = results.weights(neuronIds);

%Allocate for rates - last row will represent sum of colomns
rateOfSynapticChange = zeros(numberOfPoints, size(neuronIds, 2) + 1, 3);

for i=neuronIds 
    currentWeights = weights{i,1};
    rowSize = size(currentWeights,1);
    
    neuronConnections = connections{i,1}';
    
    cutoffPoint = -1;
    for k=1:size(neuronConnections,1)
        if(neuronConnections(k) > layerBound)
           cutoffPoint = k;
           %fprintf("Neuron %i: cutoffPoint is at %i with an ID of %i\n\n",i, k, neuronConnections(k));
           break;
        end
    end
    
    
    %Remove connections to other layers
    if(cutoffPoint ~= -1)
        currentWeights(cutoffPoint:rowSize,:) = [];
    end
    
    %Simply takes the difference of the mean of the weights of all
    %connections at the endtime and starttime
    for y=2:numberOfPoints
        %endTime = y * resolution;
        %startTime = endTime - (resolution - 1);
        %fprintf("startime:%i endtime:%i\n", startTime, endTime)
        %for k=startTime+1:endTime
            timeRangeIncrementMean = mean(currentWeights(:, y)) - mean(currentWeights(:, y-1));
            if timeRangeIncrementMean > 0
                rateOfSynapticChange(y, i, 1) = rateOfSynapticChange(y, i, 1) + timeRangeIncrementMean;
            else
                rateOfSynapticChange(y, i, 2) = rateOfSynapticChange(y, i, 2) + timeRangeIncrementMean;
            end
        %end
        rateOfSynapticChange(y,i,3) = rateOfSynapticChange(y,i,1)+ rateOfSynapticChange(y,i,2);
        if rateOfSynapticChange(y, i,3) > 1
            fprintf("rateOfSynapticChange:%i", rateOfSynapticChange(y, i,3))
        end
        
    end
end

for y=1:numberOfPoints 
    fprintf("y:%i\n", y)
    %Increases in weight
    rateOfSynapticChange(y, end, 1) = sum(rateOfSynapticChange(y, 1:end-1, 1));

    %Decreases in weight
    rateOfSynapticChange(y, end, 2) = sum(rateOfSynapticChange(y, 1:end-1, 2));

    %Sum of all
    rateOfSynapticChange(y, end, 3) = sum(rateOfSynapticChange(y, 1:end-1, 3));
end

end

