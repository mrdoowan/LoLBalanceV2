'''
Created by: Steven Duan and Daniel Jin

With the written document, this will balance teams based on a Primary and 
multiple Secondary role preferences

'''

import csv
import sys

# "Global" variables

# Only to compare Ranks together
COMPARE_RANK = {
	'bronze 5': 1,
	'bronze 4': 2,
	'bronze 3': 3,
	'bronze 2': 4,
	'bronze 1': 5,
	'silver 5': 6,
	'silver 4': 7,
	'silver 3': 8,
	'silver 2': 9,
	'silver 1': 10,
	'gold 5': 11,
	'gold 4': 12,
	'gold 3': 13,
	'gold 2': 14,
	'gold 1': 15,
	'platinum 5': 16,
	'platinum 4': 17,
	'platinum 3': 18, 
	'platinum 2': 19,
	'platinum 1': 20,
	'diamond 5': 21,
	'diamond 4': 22,
	'diamond 3': 23,
	'diamond 2': 24,
	'diamond 1': 25,
	'master': 26,
	'challenger': 27
}

# The translation from a Ranking to a Tier
# Explained in documentation
RANK_TIER = {
	'bronze 5': 1,
	'bronze 4': 1,
	'bronze 3': 1,
	'bronze 2': 1,
	'bronze 1': 2,
	'silver 5': 2,
	'silver 4': 3,
	'silver 3': 3,
	'silver 2': 4,
	'silver 1': 4,
	'gold 5': 5,
	'gold 4': 5,
	'gold 3': 6,
	'gold 2': 6,
	'gold 1': 6,
	'platinum 5': 7,
	'platinum 4': 7,
	'platinum 3': 8, 
	'platinum 2': 8,
	'platinum 1': 8,
	'diamond 5': 9,
	'diamond 4': 9,
	'diamond 3': 10,
	'diamond 2': 10,
	'diamond 1': 10,
	'master': 10,
	'challenger': 10
}

# Map of Duos that will be a global
DUOS_MAP = {}

class Player:
	# Initializer
	def __init__(self, IGN, rank, primary, second, duo):
		self.IGN = IGN				# string
		self.rank = rank 			# string
		self.primary_role = primary # string
		self.second_role = second 	# string[]
		self.duo = duo 				# string
		# add to DUOS_MAP
		if duo:
			DUOS_MAP[IGN] = duo
		# Primary fill should empty out secondary roles
		if (primary == "Fill"):
			self.second_role = []
		# Force fill if primary is in secondary and there's only one secondary
		elif ((primary in second) and (len(second) == 1)):
			self.primary_role = "Fill"
			self.second_role = []
		# If primary is in secondary, but secondary > 1, remove from secondary
		elif ((primary in second) and (len(second) > 1)):
			self.second_role.remove(primary)
		self.tier = RANK_TIER[rank]	# int

	# Only time the below is needed is if I'm using this class as a key to a dictionary
	'''
	def __hash__(self):
		return hash((self.IGN, self.rank, self.primary_role, self.second_role, self.tier))

	def __eq__(self, other):
		return ???

	def __ne__(self, other):
		return ???
	'''

#This is the int main()
if __name__ == "__main__":
	fileName = str(sys.argv[1])
	outputFile = str(sys.argv[2])
	# Parse fileName of players.
	# Do csv since these are Google Form responses anyways (for now)
	# csv should be in this order: Name, IGN, Rank, Primary, Secondaries, Duo
	origPlayers = {}
	with open(fileName) as csvFile:
		reader = csv.DictReader( csvFile, ['name', 'IGN', 'rank', 'primary', 'secondary', 'duo'] )
		for row in reader:
			origPlayers.append( row['IGN'].lower(), row )
	# origPlayers is an original copy for output
	# Reason being is because we're modifying much of what we have
	# Using lowercase to avoid case sensitive
	playerList = []
	for key, value in origPlayers.items():
		name = value['name']
		IGN = value['IGN'].lower()
		rank = value['rank'].lower()
		primary = value['primary']
		secondary = value['secondary'].split(", ")
		duo = value['duo'].lower()
		playerList.append( Player( name, IGN, rank, primary, secondary, duo ) )
	
	# Check for Duos: If their duo does not exist
	popKeys = []
	for key, value in DUOS_MAP.items():
		# Check if that value is a key in our DUOS_MAP
		if value not in DUOS_MAP:
			popKeys.append(key)
		elif DUOS_MAP[value] != key:
			popKeys.append(key)
	# Remove from DUOS_MAP
	for removeKey in popKeys:
		DUOS_MAP.pop(removeKey)

	# Determine # of Teams
	int numPlayers = len(playerList)
	if (numPlayers % 5 != 0):
		print("Number of players not divisible by 5")
		sys.exit()
	int numTeams = numPlayers / 5;

	