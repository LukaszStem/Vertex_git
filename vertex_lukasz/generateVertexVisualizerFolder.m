function [] =  generateVertexVisualizerFolder( pathname, results, connections )

%%Basic checks
if (pathname(end) ~= '\')
    pathname = sprintf('%s%s', pathname, '\');
end
[status,msg] = mkdir(pathname);

if (status ~= 1)
    fprintf("Error creating directory at %s\n", pathname);
end
listing = dir(pathname)
if (length(listing) > 2)
    error("Please make sure given directory is empty!");
end
if (size(results, 1) < 1)
    error("Please make sure results input is NOT empty!");
end

%%File creation
% Spikes
createJson(sprintf("%s%s.json", pathname, 'spikes'), results.spikes);

% LFP
createJson(sprintf("%s%s.json", pathname, 'LFP'), results.LFP);

% Params
createJson(sprintf("%s%s.json", pathname, 'params'), results.params);

% Weights (in seperate json files)
weightCount = size(results.weights, 1);
for i = 1:weightCount
    fileName = sprintf('%s%s%i.json', pathname, 'weights', i);
    createJson(fileName, results.weights(i));
end

% Pre neuron connections
for i = 1:size(results.params.RecordingSettings.weights_preN_IDs,2)
    preNeuronId = results.params.RecordingSettings.weights_preN_IDs(i)
    fileName = sprintf('%s%s%i_%i.json', pathname, 'connections', i, preNeuronId);
    createJson(fileName, connections(preNeuronId,1));
end

