// author: Simone Soso, this script use an annotation file to seperate
// single gesture from an aquisition and store them into new files
// the execution time is very long to see how the program is going 
// allow the cout line inside the file

#include <fstream>
#include <iostream>
#include <sstream>
#include <string>
#include <vector>

using namespace std;

//split function, split a string on a certain delim put the content in a std::vector<std::string>
std::vector<std::string> split(const std::string &s, char delim) {
  std::stringstream ss(s);
  std::string item;
  std::vector<std::string> elems;
  while (std::getline(ss, item, delim)) {
    elems.push_back(item);
  }
  return elems;
}

//gestureNumber function, give the number of a certain gesture
string gestureNumber(string gesture) {

  if (gesture == "ONE")
    return "1";
  if (gesture == "TWO")
    return "2";
  if (gesture == "THREE")
    return "3";
  if (gesture == "FOUR")
    return "4";
  if (gesture == "OK")
    return "5";
  if (gesture == "MENU")
    return "6";
  if (gesture == "POINTING")
    return "7";
  if (gesture == "LEFT")
    return "8";
  if (gesture == "RIGHT")
    return "9";
  if (gesture == "CIRCLE")
    return "10";
  if (gesture == "V")
    return "11";
  if (gesture == "CROSS")
    return "12";
  if (gesture == "GRAB")
    return "13";
  if (gesture == "EXPAND")
    return "14";
  if (gesture == "PINCH")
    return "15";
  if (gesture == "TAP")
    return "16";
  if (gesture == "DENY")
    return "17";
  if (gesture == "KNOB")
    return "18";
  cout << "errore";
  return "0";
}

int main() {

  //2 input file streams for results(annotation file) and currentFile(single acquisition)
  ifstream results;
  ifstream currentFile;

  //1 ouput file stream  for outputFile(every single file generated by the script)
  ofstream outputFile;

  /*3 strings 
  rigaAcquisizione is use to store line from currentFile
  rigaResults is use to store line from results
  nomeFile is use to store the name of the new file to generate with the data taken by results and currentFile
  */
  string rigaAcquisizione;
  string rigaResults;
  string nomeFile = "";

  //resultsElements is use to store the result of a split of a rigaResults
  vector<string> resultsElements;

  //tipoAcquisizione store the name of the current gesture
  string tipoAcquisizione = "";

  //inizioAq and fineAq are use to store the frame where the gesture begin/end
  int inizioAq = 0;
  int fineAq = 0;

  //inizioCiclo is use as a counter in cycles for the current line in currentFile
  int inizioCiclo = 1;

  //gesti[18] is a counter of how many times the program find a gesture, is used to get the correct name of generated File
  int gesti[18] = {0};
    
  //open the annotation file, change the name to change the file, if the file is not in the current directory
  //specify the path to the file before the name
  results.open("ResultsV2.txt");

  //this for cycles a number of times equal to the number of aqusition you want to separate in single gesture
  //for(int i= firstAcquisitionNumber, i < (lastAcquisitionNumber+1); i++)
  for (int i = 1; i < 109; i++) {
    
    //open the current aquisition file based on i counter 
    //specify the path to the acquisitions files
    currentFile.open("AcquisizioneOriginali/" + to_string(i) + ".txt");

    //get a line from the annotation file and store it in rigaResults
    getline(results, rigaResults);

    //split the line store in rigaResults using ';' as delimiter ans stores single elements in resultsElements
    resultsElements = split(rigaResults, ';');

    //reset InizioCiclo at every cycle
    inizioCiclo = 1;

    //loop over the elements in resultsElements using an Iterator j
    for (auto j = resultsElements.begin(); j != resultsElements.end(); ++j) {
      
      //ignore the first elements of resultsElements that is the acquisition number  
      if (j == resultsElements.begin())
        continue;

      //get the gesture type and store it in tipoAcquisizione then skip to the next cycle
      if (tipoAcquisizione == "") {
        if (*j == "\n" || *j == " " || *j == "")
          continue;
        tipoAcquisizione = *j;
        continue;
      }

      //get the gesture begin frame and store it in inizioAq then skip to the next cycle
      if (inizioAq == 0) {
        //stoi convert a string into an int
        inizioAq = stoi(*j);
        continue;
      }

      //get the gesture end frame and store it in fineAq, stoi convert a string into an int
      fineAq = stoi(*j);

      //print a message if gesture end is < then gesture begin
      if (inizioAq >= fineAq) {
          cout << "Errore fine < inizio " << i << " " << tipoAcquisizione << " " << inizioAq << " " << fineAq << endl;;
      }

      //print some information every cycle to see how the compilation is going, disable with //
      cout << i << " " << tipoAcquisizione << " " << inizioAq << " " << fineAq << endl;

      //this cycle get Lines from currentFile until reaching gesture begin
      for (; inizioCiclo < inizioAq; inizioCiclo++) {        
        getline(currentFile, rigaAcquisizione);
        //if he founds # ignore the current line
        if (rigaAcquisizione[0] == '#')
            inizioCiclo--;
      }

      //generate the output file name and path assuming that already exists 18 directory inside 
      //Originali (or specifiy the path), and every directory are named 1 2 3 ... 18
      nomeFile = "Originali/" + gestureNumber(tipoAcquisizione) + "/" +
          to_string(++gesti[stoi(gestureNumber(tipoAcquisizione)) - 1]) +
          ".txt";
      
      //open or generate the file named nomeFile and write the line store in rigaAcquisizione, 
      //ios::trunc allow the programe to erase whatever was store inside the file before execution
      outputFile.open(nomeFile, ios::trunc);
      getline(currentFile, rigaAcquisizione);
      outputFile << rigaAcquisizione<<"\n";
      outputFile.close();
      //inzioCiclo must be incremented cause we ve alredy wrote a line 
      inizioCiclo++;

      //the file must be closed and opend again to change the open mode to ios:app
      //to let the new lines not overwrite the first line
      outputFile.open(nomeFile, ios::app);

      //for loops over the gesture duration and wrote ever lines ot the gesture to outputFile
      for (; inizioCiclo <= fineAq; inizioCiclo++) {
        getline(currentFile, rigaAcquisizione);
        //if he founds # ignore the current line
        if (rigaAcquisizione[0] == '#') {
            inizioCiclo--;
            continue;
        }
        outputFile << rigaAcquisizione << "\n";
      }
      //every acquisition files have 2 "\n" in the end so to mantain that it stores another "\n" in ouputFile
      outputFile << "\n";
      outputFile.close();

      //resets tipoAcquisizione and inizioAq
      tipoAcquisizione = "";
      inizioAq = 0;
    }
    //close the currentFile to let the program pass to another
    currentFile.close();
  }
  //close annotation file
  results.close();
  
  return 0;
}
