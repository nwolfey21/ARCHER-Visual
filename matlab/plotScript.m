LoadData = 1;   %Load data file? 1=yes, 0=no

if(LoadData)
    clear data
    clear face
    clear vertex
    clear vertexNormal
%     close all
%     clc
%     filename = '/home/noah/Documents/ARCHER/DataVisualization/Skin.obj';
    filename = '/home/noah/Documents/ARCHER/DataVisualization/abdomen/bone.obj';
    data = importOBJ(filename);
    [vertex, vertexNormal, face] = splitOBJ(data);
end
normals = 0;     %Are there normals? 1=yes, 0=no

% %Point Plot
% figure
% hold on
% S(1:length(vertex)) = 20;
% scatter3(vertex(:,2),vertex(:,3),vertex(:,4),S)

%Volume/Surface Plot
%figure
hold on
for i=1:length(face)                
    for j=1:3
        if(normals)
            vIdx(j)  = face(i,2*j);
    %        vnIdx(j) = face(i,2*j+1);
        else
             vIdx(j)  = face(i,j);
    %        vnIdx(j) = face(i,2*j+1);
        end
    end
    if(vIdx(:) ~= 0)
        v  = vertex(vIdx,2:4);
    %    v(4,2:4) = vertex(vIdx(1),2:4);
    %    vn = vertexNormal(vnIdx,2:4);
    %    vn(4,2:4) = vertexNormal(vnIdx(1),2:4);
        fill3(v(:,1)',v(:,2)',v(:,3)','b','EdgeColor','None');
        if(mod(i,floor(0.10*length(face))) == 0)
            disp(i/length(face))
        end
    end
end
xlabel('x-axis');
ylabel('y-axis');
zlabel('z-axis');
view([0 90])      %Front
%view([0 0])      %Top
alpha(0.2)